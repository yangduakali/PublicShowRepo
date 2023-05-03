using System;

namespace UnityExt.attributes;

[AttributeUsage(AttributeTargets.Field)]
public class FoldoutAttribute : Attribute
{
    public readonly string Tag;
    public FoldoutAttribute(string tag) => this.Tag = tag;
    
}