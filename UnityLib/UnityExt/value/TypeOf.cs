using System;
// ReSharper disable IdentifierTypo

namespace UnityExt.value;


[Serializable]
public struct TypeOf<T>
{
    public string @namespace;
    public string @class;
    public string asmdef;

    public TypeOf(string ns, string cn, string asmd)
    {
        @namespace = ns;
        @class = cn;
        asmdef = asmd;
    }

    public bool IsNull() => (Type) this == null;

    public static implicit operator Type(TypeOf<T> valueType){
        return Type.GetType(valueType.@namespace + "." + valueType.@class + ", " + valueType.asmdef);
    }

    public static implicit operator TypeOf<T>(Type valueType){
        return new TypeOf<T>(valueType.Namespace, valueType.Name, valueType.Assembly.FullName);
    }
}

[Serializable]
public struct TypeOf{
    public string @namespace;
    public string @class;
    public string asmdef;

    public TypeOf(string ns, string cn, string asmd){
        @namespace = ns;
        @class = cn;
        asmdef = asmd;
    }

    public bool IsNull() => (Type)this == null;

    public static implicit operator Type(TypeOf valueType) =>
        Type.GetType(valueType.@namespace + "." + valueType.@class + ", " + valueType.asmdef);

    public static implicit operator TypeOf(Type valueType){
        return new TypeOf(valueType.Namespace, valueType.Name, valueType.Assembly.FullName);
    }
}
