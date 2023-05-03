using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;
using Application = UnityEngine.Application;
using Object = UnityEngine.Object;

// ReSharper disable UnassignedField.Global

namespace UnityEditorExt;
/// <summary>
/// Combine multiple texture in to one texture and rename each with prefix __0a1 (mean index x:0 y:1). Also change each to read and write texture asset.
/// </summary>
public class TextureCombiner{
    /// <summary>
    /// Example : "/_game/Runtime/Sprites"
    /// </summary>
    public string InputPath;
    /// <summary>
    /// Example : "Assets/_game/Runtime/Sprites/Result.png"
    /// </summary>
    public string OutputPath;
    /// <summary>
    /// Texture for fill empty. Optional.  
    /// </summary>
    public string EmptyTexture;
    /// <summary>
    /// Size of one texture. All texture must same size
    /// </summary>
    public int Size;
    
    public void Combine(Action<Texture2D> onComplete  = null){
        if (InputPath.Contains("Assets")) {
            InputPath.Replace("Assets", "");
        }
        
        if (!IsValid(out var massage)) {
            Debug.LogError(massage);
            return;
        }
        
        string[] spritePaths = Directory.GetFiles(Application.dataPath + InputPath, "*.png", SearchOption.AllDirectories);
        var count = Mathf.CeilToInt(Mathf.Sqrt(spritePaths.Length));
        var texResult = new Texture2D(Size * count, Size * count);
        var spriteAsset = spritePaths.
            Select(matFile => "Assets" + matFile.Replace(Application.dataPath, "").Replace('\\', '/')).
            Select(assetPath => (Sprite)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite))).ToList();

        for (int i = 0; i < spriteAsset.Count; i++) {
            if (spriteAsset[i].texture.isReadable) continue;
            var assetPath = AssetDatabase.GetAssetPath(spriteAsset[i]);
            var tImporter = AssetImporter.GetAtPath( assetPath ) as TextureImporter;
            if (tImporter == null) continue;
            tImporter.textureType = TextureImporterType.Sprite;
            tImporter.isReadable = true;
            AssetDatabase.ImportAsset( assetPath );
        }

        if (Directory.Exists(EmptyTexture)) {
            var noImageColor = AssetDatabase.LoadAssetAtPath<Texture2D>(EmptyTexture).GetPixels();
            for (int x = 0; x < count; x++) {
                for (int y = 0; y < count; y++) {
                    texResult.SetPixels(Size*x,Size * y,Size,Size,noImageColor);
                }   
            }
        }
        
        var ix = 0; 
        for (int i = 0; i < spriteAsset.Count; i += count) {
            int batch = Mathf.Min(spriteAsset.Count - i, count);
            for (int a = 0; a < batch; a++) {
                var index = i + a;
                var name = spriteAsset[index].name.Split("__")[0] + $"__{a}a{ix}";
                var r = AssetDatabase.LoadAssetAtPath<Sprite>(OutputPath);
                if(r != null && name.Contains(r.name)) continue;
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(spriteAsset[index]),name);
                var te = spriteAsset[index].texture.GetPixels();
                texResult.SetPixels(Size*a,Size*ix,Size,Size,te);
            }
            ix++;
        }
        
        texResult.Apply();
        byte[] bytes = texResult.EncodeToPNG();
        Object.DestroyImmediate(texResult);
        File.WriteAllBytes(OutputPath, bytes);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(OutputPath);
        onComplete?.Invoke(tex);

    }

    private bool IsValid(out string massage){
        massage = "";
        if (Size >= 16) return true;
        massage = $"{Size} < 16";
        return false;

    }
}