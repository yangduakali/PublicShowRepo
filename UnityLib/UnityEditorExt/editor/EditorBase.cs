// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.UIElements;
// using UnityExt;
// using UnityExt.attributes;
//
// namespace UnityEditorExt.editor;
//
// public abstract class EditorBase : Editor{
//     protected MethodInfo[] Methods = Array.Empty<MethodInfo>();
//     protected readonly Dictionary<string, FieldInfo> Fields = new();
//     protected Rect CurrentRect;
//     private readonly Dictionary<string, List<FieldInfo>> _titleDictionary = new();
//
//     private void OnEnable(){
//         SetClassIcon();        
//         
//         var  type = target.GetType();
//         Fields.Clear();
//         Methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
//         var includingBaseClasses = type.GetFieldInfosIncludingBaseClasses(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
//         for (var index = 0; index < includingBaseClasses.Length; ++index)
//             Fields.Add(includingBaseClasses[index].Name, includingBaseClasses[index]);
//     }
//
//     public override void OnInspectorGUI(){
//         if(target is DefaultAsset) return;
//         serializedObject.Update();
//         _titleDictionary .Clear();
//         CurrentRect = EditorGUILayout.BeginVertical();
//         OnDrawInspectorBeforeProperty();
//         EditorGUILayout.EndVertical();
//         var iterator = serializedObject.GetIterator();
//         if (iterator.Next(true))
//         {
//             do
//             {
//                 if (iterator.name == "m_Script")
//                 {
//                     CurrentRect = EditorGUILayout.BeginVertical();
//                     GUI.enabled = false;
//                     var prop = serializedObject.FindProperty("m_Script");
//                     EditorGUILayout.PropertyField(prop, true);
//                     GUI.enabled = true;
//                     EditorGUILayout.EndVertical();
//
//                 }
//                 if (!Fields.TryGetValue(iterator.name, out var filedInfo)) continue;
//                 CurrentRect = EditorGUILayout.BeginVertical();
//                 OnDrawProperty(serializedObject.FindProperty(iterator.name), filedInfo);
//                 EditorGUILayout.EndVertical();
//             }
//             while (iterator.NextVisible(false));
//         }
//         OnDrawInspectorAfterProperty();
//         serializedObject.ApplyModifiedProperties();
//     }
//     protected virtual void OnDrawInspectorBeforeProperty(){
//         DrawClassInfo();        
//     }
//     protected virtual void OnDrawProperty(SerializedProperty prop, FieldInfo filedInfo){
//         if (IsHide(filedInfo) || IsInlineScriptableObject(prop,filedInfo) || IsContainTitle(filedInfo) )return;
//         GUI.enabled = !IsReadOly(filedInfo);
//         EditorGUILayout.PropertyField(prop);
//         GUI.enabled = true;
//     }
//     protected virtual void OnDrawInspectorAfterProperty(){
//         DrawCustomButtons(Methods);
//     }
//     
//     private void SetClassIcon(){
//         if(!target.GetType().TryGetCustomAttribute<ClassIconAttribute>(out var att))return;
//         var fullPath = $"UnityEditorExt.Resources.{att.Icon}.png"; 
//         var asm = Assembly.GetExecutingAssembly();
//         if (!asm.GetManifestResourceNames().Contains(fullPath)) { return; }
//
//         byte[] iconBytes;
//         using (var stream = asm.GetManifestResourceStream(fullPath))
//             iconBytes = stream.ReadAllBytes();
//         var icon = new Texture2D(128, 128);
//         icon.LoadImage(iconBytes);
//         EditorGUIUtility.SetIconForObject(target,icon);
//     }
//     
//     protected void DrawClassInfo(){
//         IEnumerable<InfoClassAttribute> att = target.GetType().GetCustomAttributes<InfoClassAttribute>();
//         var infoClassAttributes = att as InfoClassAttribute[] ?? att.ToArray();
//         if (!infoClassAttributes.Any()) return;
//         int num = infoClassAttributes.Count();
//         for (int index = 0; index < num; ++index) {
//             var info = infoClassAttributes.ElementAt(index);
//             if (info.IsCondition) {
//                 var methodInfo = Methods.FirstOrDefault(x => x.ReturnParameter.ParameterType == typeof(bool) && x.Name == info.Condition);
//                 if (methodInfo != null && (bool)methodInfo.Invoke(target, null)) {
//                     EditorGUILayout.HelpBox(info.InfoText, (MessageType)info.InfoType);
//                 }
//             }
//             else {
//                 EditorGUILayout.HelpBox(info.InfoText, (MessageType)info.InfoType);
//             }
//         }
//     }
//     protected void DrawCustomButtons(MethodInfo[] methods)
//     {
//         for (var i = 0; i < methods.Length; ++i)
//         {
//             var method = methods[i];
//             if (method.GetCustomAttribute<ButtonAttribute>() == null) continue;
//             if (GUILayout.Button(method.Name)) {
//                 method.Invoke(target, null);
//             }
//         }
//     }
//     protected bool IsHide(FieldInfo filedInfo)
//     {
//         var hidAtt = filedInfo.GetCustomAttribute<HideAttribute>();
//         if (hidAtt == null) return false;
//         if (!hidAtt.IsCondition) return true;
//         var methodInfo = Methods.FirstOrDefault(x => x.ReturnParameter.ParameterType == typeof (bool) && x.Name == hidAtt.Condition);
//         return methodInfo != null && (bool) methodInfo.Invoke(target, null);
//     }
//     protected bool IsReadOly(FieldInfo filedInfo){
//         return filedInfo.GetCustomAttribute<ReadOnlyAttribute>() != null;
//     }
//     protected bool IsContainTitle(FieldInfo filedInfo){
//         if (!filedInfo.TryGetCustomAttribute<TitleAttribute>(out var att)) {
//             return false;
//         }
//         if (string.IsNullOrWhiteSpace(att.TitleText)) return false;
//         if (_titleDictionary.ContainsKey(att.TitleText)) {
//             return _titleDictionary[att.TitleText].Contains(filedInfo);
//         }
//         GUILayout.Space(5);
//         var style = new GUIStyle {
//             fontSize = 12,
//             alignment = TextAnchor.MiddleLeft,
//             normal = {
//                 textColor = ColorExt.UnityTextColor()
//             }
//         };
//         EditorExt.DrawUIBox(CurrentRect.WithHeight(EditorGUIUtility.singleLineHeight + 5).WithOffsetX(5* EditorGUI.indentLevel),ColorExt.UnityTextColor(),ColorExt.UnityWindow() ,0,0.3f,0,0);
//         string s = "";
//         for (int i = 0; i < EditorGUI.indentLevel; i++) {
//             s += "  ";
//         }
//         GUILayout.Label($"<b><color=#B3B3B3>{s}{att.TitleText}</color></b>",style);
//         GUILayout.Space(2);
//         
//         _titleDictionary.Add(att.TitleText,new List<FieldInfo>());
//         foreach (var f in Fields) {
//             if (!f.Value.TryGetCustomAttribute<TitleAttribute>(out var otherAtt)) continue;
//             if(otherAtt.TitleText != att.TitleText) continue;
//             var prop = serializedObject.FindProperty(f.Value.Name);
//             OnDrawProperty(prop,f.Value);
//             _titleDictionary[att.TitleText].Add(f.Value);
//         }        
//         return true;
//     }
//     protected bool IsInlineScriptableObject(SerializedProperty prop, FieldInfo filedInfo){
//         if (filedInfo.GetValue(target) == null) {
//             prop.isExpanded = false;
//             return false;
//         }
//
//         if (filedInfo.GetCustomAttribute<InlineScriptableObjectAttribute>() == null
//             || !prop.objectReferenceValue.GetType().IsSubclassOf(typeof(ScriptableObject))) {
//             prop.isExpanded = false;
//             return false;
//         }
//
//         EditorGUI.BeginChangeCheck();
//         
//         var rect = CurrentRect;
//         rect.x = EditorGUI.indentLevel* 5;
//         rect.height = EditorGUIUtility.singleLineHeight;
//         if (GUI.Button(rect.WithWidth(EditorGUIUtility.labelWidth), "")) {
//             prop.isExpanded = !prop.isExpanded;
//         }
//         EditorExt.DrawUIBox(CurrentRect.WithX(EditorGUI.indentLevel* 5),Color.black, ColorExt.UnityWindow(), 0,0.5f,1,0);
//         EditorExt.DrawUIBox(rect,Color.black, ColorExt.UnityDarkGrey(),1,0,1,0);
//         GUI.enabled = !IsReadOly(filedInfo);
//         EditorGUILayout.PropertyField(prop);
//         int currentIndentLevel = EditorGUI.indentLevel;
//
//         if (EditorGUI.EndChangeCheck()) {
//             
//             if (filedInfo.GetValue(target) == null) {
//                 prop.isExpanded = false;
//                 return false;
//             }
//             if (prop.isExpanded) {
//                 EditorGUI.indentLevel++;
//                 var e = CreateEditor(prop.objectReferenceValue);
//                 e.OnInspectorGUI();
//                 GUILayout.Space(5);
//             }        
//             
//         }
//         //if (prop.isExpanded) {
//         //    EditorGUI.indentLevel++;
//         //    var e = CreateEditor(prop.objectReferenceValue);
//         //    e.OnInspectorGUI();
//         //    GUILayout.Space(5);
//         //}        
//         EditorGUI.indentLevel = currentIndentLevel;
//         GUI.enabled = true;
//         return true;
//     }
//     
//     
//     public override VisualElement CreateInspectorGUI(){
//         return new IMGUIContainer(OnInspectorGUI);
//     }
// }