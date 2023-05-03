using System;

namespace UnityExt.attributes;

[AttributeUsage(AttributeTargets.Field)]
public class MinSliderAttribute : Attribute
{
    public readonly string Tag;
    public readonly float MinLimit;

    public MinSliderAttribute(string tag, float minLimit)
    {
        Tag = tag;
        MinLimit = minLimit;
    }
}