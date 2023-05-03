using System;

namespace UnityExt.attributes;

[AttributeUsage(AttributeTargets.Class)]
public class InspectorStyleAttribute : Attribute{
    public InspectorStyleAttribute(string getTexture){
        GetTextureMethodName = getTexture;
    }
        
    public readonly string GetTextureMethodName;
        
}