using System;

namespace UnityExt.attributes;

[AttributeUsage(AttributeTargets.Field)]
public class TitleAttribute : Attribute{
    public TitleAttribute(string titleText){
        TitleText = titleText;
    }
    public readonly string TitleText;
}