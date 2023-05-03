using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityExt.attributes;

namespace UnityEditorExt.toolbar;
[InitializeOnLoad]
internal static class ToolbarExtender {
    private  const BindingFlags BindingAttr = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
    private static readonly List<(LeftToolbarButtonAttribute, MethodInfo)> ListLeft = new();
    private static readonly List<(RightToolbarButtonAttribute, MethodInfo)> ListRight = new();

    static ToolbarExtender()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingAttr))
                {
                    if (method.TryGetCustomAttribute<LeftToolbarButtonAttribute>(out var leftAtt))
                    {
                        ListLeft.Add((leftAtt, method));
                    }
                    else if (method.TryGetCustomAttribute<RightToolbarButtonAttribute>(out var rightAtt)) {
                        ListRight.Add((rightAtt, method));
                    }

                }
            }
        }
        ListLeft.OrderBy((Func<(LeftToolbarButtonAttribute, MethodInfo), int>) (x => x.Item1.Order));
        ListRight.OrderBy((Func<(RightToolbarButtonAttribute, MethodInfo), int>) (x => x.Item1.Order));
        ToolbarCallback.OnToolbarGUILeft = () =>
        {
            GUILayout.BeginHorizontal();
            for (int index = 0; index < ListLeft.Count; ++index)
            {
                var methodInfo = ListLeft[index].Item2;
                var toolbarButtonAttribute = ListLeft[index].Item1;
                if (GUILayout.Button(string.IsNullOrEmpty(toolbarButtonAttribute.Name) ? methodInfo.Name : toolbarButtonAttribute.Name))
                    methodInfo.Invoke(null, null);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        };
        ToolbarCallback.OnToolbarGUIRight = () =>
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int index = 0; index < ListRight.Count; ++index)
            {
                var methodInfo = ListRight[index].Item2;
                var toolbarButtonAttribute = ListRight[index].Item1;
                if (GUILayout.Button(string.IsNullOrEmpty(toolbarButtonAttribute.Name) ? methodInfo.Name : toolbarButtonAttribute.Name))
                    methodInfo.Invoke(null, null);
            }
            GUILayout.EndHorizontal();
        };
    }
}
