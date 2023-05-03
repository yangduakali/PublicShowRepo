using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditorExt.toolbar;

public static class ToolbarCallback
{
    private static readonly Type MToolbarType = typeof (Editor).Assembly.GetType("UnityEditor.Toolbar");
    private static ScriptableObject _mCurrentToolbar;
    public static Action OnToolbarGUILeft;
    public static Action OnToolbarGUIRight;

    static ToolbarCallback()
    {
        EditorApplication.update -= OnUpdate;
        EditorApplication.update += OnUpdate;
    }

    private static void OnUpdate()
    {
        if (!(_mCurrentToolbar == null))
            return;
        UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(MToolbarType);
        _mCurrentToolbar = objectsOfTypeAll.Length != 0 ? (ScriptableObject) objectsOfTypeAll[0] : null;
        if (_mCurrentToolbar == null) return;
        var mRoot = _mCurrentToolbar.GetType().GetField("m_Root", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_mCurrentToolbar) as VisualElement;
        RegisterCallback("ToolbarZoneLeftAlign", OnToolbarGUILeft, true);
        RegisterCallback("ToolbarZoneRightAlign", OnToolbarGUIRight, false);

        void RegisterCallback(string root, Action cb, bool isLeft)
        {
            var visualElement = mRoot.Q(root);
            var child1 = new VisualElement()
            {
                style = {
                    flexGrow = 1f,
                    marginLeft = isLeft ? 10f : 0.0f,
                    marginRight = isLeft ? 0.0f : 10f
                }
            };
            var child2 = new IMGUIContainer();
            child2.onGUIHandler += (Action) (() =>
            {
                cb?.Invoke();
            });
            child1.Add(child2);
            visualElement.Add(child1);
        }
    }
}