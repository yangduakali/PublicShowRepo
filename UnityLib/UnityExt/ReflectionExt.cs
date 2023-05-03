using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityExt;

public static class ReflectionExt{
    public static List<Type> GetAllType(Type type)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        List<Type> allType = new List<Type>();
        foreach (var assembly in assemblies)
        {
            Type[] types = assembly.GetTypes();
            int length = types.Length;
            for (int index = 0; index < length; ++index)
            {
                if (types[index].IsSubclassOf(type))
                    allType.Add(types[index]);
            }
        }
        return allType;
    }

    public static FieldInfo[] GetFieldInfosIncludingBaseClasses(this Type type, BindingFlags bindingFlags){
        return type.GetFieldInfosIncludingBaseClasses(bindingFlags, typeof (object));
    }

    public static FieldInfo[] GetFieldInfosIncludingBaseClasses( this Type type, BindingFlags bindingFlags, Type maxIterate){
        FieldInfo[] fields1 = type.GetFields(bindingFlags);
        if (type.BaseType == maxIterate)
            return fields1;
        var targetType = type;
        var comparer = new FieldInfoComparer();
        HashSet<FieldInfo> source = new HashSet<FieldInfo>(fields1, comparer);
        for (; targetType != maxIterate; targetType = targetType.BaseType)
        {
            FieldInfo[] fields2 = targetType.GetFields(bindingFlags);
            source.UnionWith(fields2);
        }
        return source.ToArray();
    }
}

public class FieldInfoComparer : IEqualityComparer<FieldInfo>
{
    public bool Equals(FieldInfo x, FieldInfo y) => x.DeclaringType == y.DeclaringType && x.Name == y.Name;
    public int GetHashCode(FieldInfo obj) => obj.Name.GetHashCode() ^ obj.DeclaringType.GetHashCode();
}