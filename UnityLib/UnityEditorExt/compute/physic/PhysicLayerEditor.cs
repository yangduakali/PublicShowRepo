using UnityEditor;
using UnityEngine;
using UnityExt;
using UnityExt.compute.physic;

namespace UnityEditorExt.compute.physic;

[CustomEditor(typeof(PhysicLayer))]
public class PhysicLayerEditor : Editor{
    private PhysicLayer _layer;
    private SerializedProperty _layerNameProp;
    private SerializedProperty _layerValueProp;

    private int[] _layerValue;
    private bool[] _isValidName;
    private void OnEnable(){
        _isValidName = new bool[10];
        _layerValue = new int[100];
        _layer = (PhysicLayer)target;
        _layerNameProp = serializedObject.FindProperty("layerName");
        _layerValueProp = serializedObject.FindProperty("layerValue");

        if (_layerNameProp.arraySize == 0) {
            _layerNameProp.arraySize = 10;
            _layerNameProp.GetArrayElementAtIndex(0).stringValue = "Default";
            serializedObject.ApplyModifiedProperties();
        }

        if (_layerValueProp.arraySize != 100) {
            for (int i = 0; i < _layerValue.Length; i++) {
                if (i < _layerValueProp.arraySize - 1) {
                    _layerValue[i] = _layerValueProp.GetArrayElementAtIndex(i).intValue;
                    continue;
                }
                _layerValue[i] = 0;
            }
            _layerValueProp.arraySize = 100;
            for (int i = 0; i < _layerValue.Length; i++) {
                _layerValueProp.GetArrayElementAtIndex(i).intValue = _layerValue[i];
            }
            
        }
        else {
            for (int i = 0; i < 100; i++) {
                _layerValue[i] = _layerValueProp.GetArrayElementAtIndex(i).intValue; 
            }
        }
    }

    public override void OnInspectorGUI(){
        EditorGUI.BeginChangeCheck();
        GUI.enabled = false;
        EditorGUILayout.TextField("Layer 0", "Default");
        GUI.enabled = true;
        
        for (int i = 1; i < 10; i++) {
            var l =  EditorGUILayout.TextField($"Layer {i}", _layerNameProp.GetArrayElementAtIndex(i).stringValue);
            _layerNameProp.GetArrayElementAtIndex(i).stringValue = l;
        }

        GUILayout.Space(100);
        
        GUILayout.BeginArea(new Rect(118, 300, 100, 400));
        var rects =  EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();
        GUIUtility.RotateAroundPivot(-90,new Vector2(rects.x,rects.y));
        for (int i = 9; i >= 0; i--) {
            _isValidName[i] = false;
            if (string.IsNullOrEmpty(_layerNameProp.GetArrayElementAtIndex(i).stringValue)) {
                continue;
            }

            GUILayout.Label( _layerNameProp.GetArrayElementAtIndex(i).stringValue,GUILayout.Height(18));
            _isValidName[i] = true;
        }
        GUIUtility.RotateAroundPivot(90,new Vector2(rects.x,rects.y));
        GUILayout.EndArea();

        bool v = false;
        for (int y = 0; y < 10; y++) {
            if(string.IsNullOrEmpty( _layerNameProp.GetArrayElementAtIndex(y).stringValue))continue;
            GUILayout.BeginHorizontal();            
            for (int x = 9; x >= 0 ; x--) {
                if (x == 9) {
                    GUILayout.Label( _layerNameProp.GetArrayElementAtIndex(y).stringValue,GUILayout.Width(100));
                    if (!_isValidName[x]) {
                        _layerValue[y * 10 + x] = 0;
                        _layerValue[x * 10 + y] = 0;
                        continue;                        
                    }

                    if (x < y) {
                         _layerValue[x * 10 + y] = _layerValue[y * 10 + x];     
                        continue;
                    }
                    v = _layerValue[y * 10 + x] == 1;
                    v =  GUILayout.Toggle(v , content: new GUIContent(""));
                    _layerValue[y * 10 + x] = v ? 1 : 0;
                    _layerValue[x * 10 + y] = v ? 1 : 0;
                    
                    continue;
                }
                if (!_isValidName[x]) {
                    _layerValue[y * 10 + x] = 0;
                    _layerValue[x * 10 + y] = 0;

                    continue;                        
                }
                if (x < y) {
                    _layerValue[x * 10 + y] = _layerValue[y * 10 + x];     
                    continue;
                }
                v = _layerValue[y * 10 + x] == 1;
                v =  GUILayout.Toggle(v , content: new GUIContent(""));
                _layerValue[y * 10 + x] = v ? 1 : 0;
                _layerValue[x * 10 + y] = v ? 1 : 0;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();            
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Select All", GUILayout.Width(150))) {
            for (var i = 0; i < _layerValue.Length; i++) {
                _layerValue[i] = 1;
            }
        }
        if (GUILayout.Button("Deselect All", GUILayout.Width(150))) {
            for (var i = 0; i < _layerValue.Length; i++) {
                _layerValue[i] = 0;
            }
        }

        
        for (int i = 0; i < _layerValue.Length; i++) {
            _layerValueProp.GetArrayElementAtIndex(i).intValue = _layerValue[i];
        }
        
        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }

    }
}