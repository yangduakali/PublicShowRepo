using System;

namespace UnityExt.attributes;

[AttributeUsage(AttributeTargets.Field)]
public class HideAttribute : Attribute
{
    public readonly bool IsCondition;
    public readonly string Condition;

    public HideAttribute()
    {
        IsCondition = false;
        Condition = "";
    }

    public HideAttribute(string condition)
    {
        IsCondition = true;
        Condition = condition;
    }
}