using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityExt.game.gameobject;
using CharacterController = UnityExt.game.gameobject.CharacterController;

namespace UnityEditorExt.game.core.PopWindow;

public class CreateGameObjectWindow : EditorWindow{

    private TextField _prefabNameInput;
    private TextField _scriptNameInput;
    private VisualElement _infoNameInput;
    private Button _bCreateActor; 
    private Button _bCreatePawn; 
    private Button _bCreateCharacter; 
    private Button _bCreateCharacterController;
    private bool _isCreateScript;
    
    public void CreateGUI(){
        _bCreateActor = new Button { text = "Actor",focusable = false};
        _bCreateActor.clicked += CreateActor;
        rootVisualElement.Add(_bCreateActor);

        _bCreatePawn = new Button { text = "Pawn" ,focusable = false};
        rootVisualElement.Add(_bCreatePawn);
        _bCreatePawn.clicked += CreatePawn;
        
        _bCreateCharacter = new Button { text = "Character",focusable = false };
        rootVisualElement.Add(_bCreateCharacter);
        _bCreateCharacter.clicked += CreateCharacter;
        
        _bCreateCharacterController = new Button { text = "CharacterController" ,focusable = false};
        _bCreateCharacterController.clicked += CreateCharacterController;
        rootVisualElement.Add(_bCreateCharacterController);
        
        _infoNameInput = new VisualElement();
        rootVisualElement.Add(_infoNameInput);
        
        _prefabNameInput = new TextField(label: "PrefabName"){ value = "p_"};
        _prefabNameInput.RegisterValueChangedCallback(OnInputNameChange);
        rootVisualElement.Add(_prefabNameInput);

        _scriptNameInput = new TextField(label: "ScriptName"){ value = "NewMono"};
        _scriptNameInput.RegisterValueChangedCallback(OnInputScriptNameChange);
        var toggleCreateScript = new Toggle(){text = "Create Script"};
        rootVisualElement.Add(toggleCreateScript);
        _isCreateScript = toggleCreateScript.value;
        if (toggleCreateScript.value) {
            rootVisualElement.Add(_scriptNameInput);
        }
        
        toggleCreateScript.RegisterValueChangedCallback(x => {
            _isCreateScript = x.newValue;
            if (x.newValue) {
                if (_scriptNameInput.parent != rootVisualElement) {
                    rootVisualElement.Add(_scriptNameInput);
                }                
                return;
            }
            if (_scriptNameInput.parent != rootVisualElement) return;
            rootVisualElement.Remove(_scriptNameInput);
            
        });
    }

    private void OnInputScriptNameChange(ChangeEvent<string> evt){
        
    }

    private void OnInputNameChange(ChangeEvent<string> evt){
        _bCreateActor.SetEnabled(!string.IsNullOrWhiteSpace( evt.newValue));
        _bCreatePawn.SetEnabled(!string.IsNullOrWhiteSpace( evt.newValue));
        _bCreateCharacter.SetEnabled(!string.IsNullOrWhiteSpace( evt.newValue));
        _bCreateCharacterController.SetEnabled(!string.IsNullOrWhiteSpace( evt.newValue));
    }

    private void CreateActor(){
        if (_isCreateScript) {
            CreateScript(typeof(Actor));
        }
        CreatePrefab(x => { },_isCreateScript? Type.GetType(_scriptNameInput.value) : null);
    }
    private void CreatePawn(){
        if (_isCreateScript) {
            CreateScript(typeof(Pawn));
        }
        CreatePrefab(x => { },_isCreateScript? Type.GetType(_scriptNameInput.value) : null);
    }
    private void CreateCharacter(){
        if (_isCreateScript) {
            CreateScript(typeof(Character));
        }
        CreatePrefab(x => { },_isCreateScript? Type.GetType(_scriptNameInput.value) : null);

    }
    private void CreateCharacterController(){
        if (_isCreateScript) {
            CreateScript(typeof(CharacterController));
        }
        CreatePrefab(x => { },_isCreateScript? Type.GetType(_scriptNameInput.value) : null);
    }
    private void CreatePrefab(Action<GameObject> onCreate,Type? typeClass = null){
        var n = typeClass != null ? new GameObject($"{_prefabNameInput.value}",typeClass) : new GameObject($"{_prefabNameInput.value}");
        onCreate(n);
        var path = EditorExt.GetCurrentFolderPath() + $"/{_prefabNameInput.value}.prefab";
        var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
        PrefabUtility.SaveAsPrefabAsset(n, uniquePath, out var isSuccess);
        DestroyImmediate(n);
        var p = AssetDatabase.LoadAssetAtPath<GameObject>(uniquePath);
        Selection.activeGameObject = p;
    }
    private void CreateScript(Type parent){
        string path = AssetDatabase.GenerateUniqueAssetPath(EditorExt.GetCurrentFolderPath() + $"/{_scriptNameInput.value}.cs");
        if( File.Exists(path) == false ) {
            using var outfile = new StreamWriter(path);
            outfile.WriteLine("using UnityExt.game.gameobject;");
            if (parent == typeof(CharacterController)) {
                outfile.WriteLine("using CharacterController = UnityExt.game.gameobject.CharacterController;");
            }
            outfile.WriteLine("");
            outfile.WriteLine($"public class "+_scriptNameInput.value+$" : {parent.Name} {{");
            outfile.WriteLine(" ");
            outfile.WriteLine(" ");
            outfile.WriteLine("}");
        }
        AssetDatabase.Refresh();
    }
}   