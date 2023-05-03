using System;

namespace UnityExt.attributes;

[AttributeUsage(AttributeTargets.Field)]
public class MaxSliderAttribute : Attribute
{
    public readonly string Tag;
    public readonly float MaxLimit;

    public MaxSliderAttribute(string tag, float maxLimit)
    {
        Tag = tag;
        MaxLimit = maxLimit;
    }
}