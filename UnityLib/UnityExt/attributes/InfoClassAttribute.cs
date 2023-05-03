using System;

namespace UnityExt.attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class InfoClassAttribute : Attribute
{
    public readonly string InfoText;
    public readonly bool IsCondition;
    public readonly string Condition;
    public readonly InfoType InfoType;

    public InfoClassAttribute(string info, InfoType infoType = InfoType.None)
    {
        InfoText = info;
        InfoType = infoType;
        
        IsCondition = false;
        Condition = "";
    }

    public InfoClassAttribute(string info, string condition, InfoType infoType = InfoType.None)
    {
        InfoText = info;
        InfoType = infoType;
        
        IsCondition = true;
        Condition = condition;
    }
}