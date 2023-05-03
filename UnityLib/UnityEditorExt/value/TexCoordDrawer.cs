using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExt.value;

namespace UnityEditorExt.value;
[CustomPropertyDrawer(typeof(TexCoord))]
public class TexCoordDrawer : PropertyDrawer{
    private Sprite _sprite;
        
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
        Draw(property);
    }
    
    private void Draw(SerializedProperty property){
        var vProp = property.FindPropertyRelative("cord");
        var guidProp = property.FindPropertyRelative("assetGuid");
        if (!string.IsNullOrWhiteSpace(guidProp.stringValue)) {
            _sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guidProp.stringValue));
            if (_sprite == null) {
                vProp.vector4Value = Vector4.zero;
                guidProp.stringValue = "";
                property.serializedObject.ApplyModifiedProperties();
                return;
            }

            var l = _sprite.name.Split("__").Length;
            if (l == 1) {
                EditorGUILayout.HelpBox("Sprite not support for material.",MessageType.Error);
            }
        }
        
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();
        _sprite =  EditorGUILayout.ObjectField(property.displayName,_sprite,typeof(Sprite),false,GUILayout.Height(EditorGUIUtility.singleLineHeight),GUILayout.Width( EditorGUIUtility.currentViewWidth - 130)) as Sprite;
        if (EditorGUI.EndChangeCheck()) {
            if (_sprite == null) {
                vProp.vector4Value = Vector4.zero;
                guidProp.stringValue = "";
                property.serializedObject.ApplyModifiedProperties();
            }
            else {
                guidProp.stringValue = EditorExt.AssetToGuid(_sprite);   
                var l = _sprite.name.Split("__").Length;
                if (l == 1) {
                    EditorGUILayout.HelpBox("Sprite not support. must prefix  __[NUMBER]a[NUMBER] ",MessageType.Error);
                }
                else {
                    vProp.vector2Value = GetCord(_sprite.name.Split("__")[1]);
                }
            }
            
            property.serializedObject.ApplyModifiedProperties();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.Box(_sprite == null? Texture2D.blackTexture: _sprite.texture,GUILayout.Width(100),GUILayout.Height(100));
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
        return EditorGUIUtility.singleLineHeight - 20;
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property){
        return new IMGUIContainer(()=> Draw(property));
            
    }
    
    private Vector2 GetCord(string text){
        var n = text.Split('a');
        return new Vector2(float.Parse(n[0]), float.Parse(n[1]));
    }

}