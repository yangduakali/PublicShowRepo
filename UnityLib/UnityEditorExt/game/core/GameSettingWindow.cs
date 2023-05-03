using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace UnityEditorExt.game.core;

public class GameSettingWindow : EditorWindow{

    [MenuItem("Game/Setting")]
    private static void OpenWindow(){
        GetWindow<GameSettingWindow>("Game Setting").Show();
    }

    public void CreateGUI()
    {
        rootVisualElement.Add(new Label("Hello"));
    }
}