using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityExt;
using UnityExt.value;

namespace UnityEditorExt ;
public static class EditorExt {

    public static bool TryGetCustomAttribute<T>(this MemberInfo memberInfo, out T attribute) where T : Attribute {
        attribute = null;
        if (memberInfo.GetCustomAttribute<T>() == null) return false;
        attribute = memberInfo.GetCustomAttribute<T>();
        return true;
    }
    public static List<T> LoadAllScriptableObject<T>() where T : ScriptableObject{
        return AssetDatabase.FindAssets("t: " + typeof(T).Name).ToList<string>()
            .Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>).ToList();
    }
    public static string AssetToGuid(UnityEngine.Object @object){
        return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(@object));
    }
    public static T GuidToAsset<T>(string guid) where T : UnityEngine.Object{
        return (T)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(T));
    }
    public static Rect GetEditorMainWindowPos()
    {
        var containerWinType = ReflectionExt.GetAllType(typeof(ScriptableObject)).FirstOrDefault(t => t.Name == "ContainerWindow");
        if (containerWinType == null)
            throw new MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");
        var showModeField = containerWinType.GetField("m_ShowMode", BindingFlags.NonPublic | BindingFlags.Instance);
        var positionProperty = containerWinType.GetProperty("position", BindingFlags.Public | BindingFlags.Instance);
        if (showModeField == null || positionProperty == null)
            throw new MissingFieldException("Can't find internal fields 'm_ShowMode' or 'position'. Maybe something has changed inside Unity");
        var windows = Resources.FindObjectsOfTypeAll(containerWinType);
        foreach (var win in windows)
        {
            var showMode = (int)showModeField.GetValue(win);
            if (showMode != 4) continue; // main window
            var pos = (Rect)positionProperty.GetValue(win, null);
            return pos;
        }
        throw new NotSupportedException("Can't find internal main window. Maybe something has changed inside Unity");
    }
    public static string GetCurrentFolderPath(){
        var projectWindowUtilType = typeof(ProjectWindowUtil);
        var getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
        object obj = getActiveFolderPath.Invoke(null, Array.Empty<object>());
        return  obj.ToString();
    }
    public static void DrawUIBox(Rect rect, Color borderColor, Color backgroundColor,float top,float button,float left, float right)
    {
        var outer = new Rect(rect);
        var inner = new Rect(rect.x + left, rect.y + top, rect.width - right * 2, rect.height - button * 2);
        EditorGUI.DrawRect(outer, borderColor);
        EditorGUI.DrawRect(inner, backgroundColor);
    }
    
    public static byte[] ReadAllBytes(this Stream stream)
    {
        long originalPosition = 0;
 
        if (stream.CanSeek)
        {
            originalPosition = stream.Position;
            stream.Position = 0;
        }
 
        try
        {
            byte[] readBuffer = new byte[4096];
 
            int totalBytesRead = 0;
            int bytesRead;
 
            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;
 
                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }
 
            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
            return buffer;
        }
        finally
        {
            if (stream.CanSeek)
            {
                stream.Position = originalPosition;
            }
        }
    }

    public static TexCoord Refresh(this TexCoord texCoord){
        var t = new TexCoord();
        if(string.IsNullOrWhiteSpace(texCoord.assetGuid)) return t;

        var assetPath = AssetDatabase.GUIDToAssetPath(texCoord.assetGuid);
        var asset = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        if(asset == null) return t;
            
        var l = asset.name.Split("__").Length;
        if (l == 1) {
            t.assetGuid = "";
            t.cord = Vector2.zero;
        }
        else {
            t.cord = GetCord(asset.name.Split("__")[1]);
        }
        t.assetGuid = texCoord.assetGuid;
        Vector2 GetCord(string text){
            var n = text.Split('a');
            return new Vector2(float.Parse(n[0]), float.Parse(n[1]));
        }
        return t;

    }
}
                              