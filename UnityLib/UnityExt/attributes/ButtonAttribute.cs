using System;

namespace UnityExt.attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field)]
public class TexturePreviewAttribute : Attribute { }
