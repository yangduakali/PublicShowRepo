using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExt.value;
// ReSharper disable StringLiteralTypo
namespace UnityEditorExt.value;
[CustomPropertyDrawer(typeof (TypeOf<>))]
internal class TypeOfTDrawer : PropertyDrawer
{
    private int _currentIndex;
    private readonly List<Type> _contentPages = new();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
      Draw(property);
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        _contentPages.Clear();
        return new IMGUIContainer(() => {
            Draw(property);
        });
    }
    
    private void Draw(SerializedProperty property){
        _contentPages.Clear();
        var t = fieldInfo.FieldType.GetGenericArguments()[0];
        Type[] types = t.Assembly.GetTypes();
        int length = types.Length;
        for (int index = 0; index < length; ++index) {
            if (!types[index].IsSubclassOf(t)) continue;
            _contentPages.Add(types[index]);
        }
        var currentTypeOf = new TypeOf()
        {
            asmdef = property.FindPropertyRelative("asmdef").stringValue,
            @class = property.FindPropertyRelative("class").stringValue,
            @namespace = property.FindPropertyRelative("namespace").stringValue
        };
        
        EditorGUI.BeginChangeCheck();
        if (_contentPages.Count > 0)
        {
            var type = (Type) currentTypeOf;
            _currentIndex = !(type != null) ? 0 : _contentPages.IndexOf(type);
            _currentIndex = EditorGUILayout.Popup(property.displayName, _currentIndex, 
                _contentPages.ConvertAll((Converter<Type, string>) (x => x.AssemblyQualifiedName.Split(',')[0])).ToArray());
        }
        else
        {
            GUI.enabled = false;
            EditorGUILayout.TextField(property.displayName, "Not Found type of " + t.Name + " on this project");
            GUI.enabled = true;
            _currentIndex = -1;
        }
        if (_currentIndex > -1)
        {
            property.FindPropertyRelative("asmdef").stringValue = _contentPages[_currentIndex].Assembly.FullName;
            property.FindPropertyRelative("class").stringValue = _contentPages[_currentIndex].Name;
            property.FindPropertyRelative("namespace").stringValue = _contentPages[_currentIndex].Namespace;
        }
        if (!EditorGUI.EndChangeCheck())
            return;
        property.serializedObject.ApplyModifiedProperties();
    }

}