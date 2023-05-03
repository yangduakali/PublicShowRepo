using UnityEditor;
using UnityEditorExt.game.core.PopWindow;
using UnityEngine;

namespace UnityEditorExt.game.core;

public class EditorOptions{

    [MenuItem("Assets/Game/Create Prefab")]
    private static void CreatePrefab(){
        var win = EditorWindow.GetWindow<CreateGameObjectWindow>("Create Prefab");
        win.maxSize = new Vector2(600,400);
        var main = EditorExt.GetEditorMainWindowPos();
        var pos = win.position;
        float w = (main.width - pos.width)*0.5f;
        float h = (main.height - pos.height)*0.5f;
        pos.x = main.x + w;
        pos.y = main.y + h;
        win.position = pos;
    }
}