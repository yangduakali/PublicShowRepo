using System;
using System.Collections.Generic;

namespace UnityExt.stat;

internal static class StatUltis<T> where T : Enum
{
    private static readonly Dictionary<Type, Queue<Modifier<T>>> Dic = new();

    public static T1 GetModifier<T1>(StatBuilder<T> builder) where T1 : Modifier<T>, new()
    {
        if (Dic.ContainsKey(typeof (T1)))
        {
            if (Dic[typeof (T1)].Count == 0)
            {
                var obj = new T1 {
                    Builder = builder
                };
                obj.Release = () => Dic[typeof (T1)].Enqueue(obj);
                return obj;
            }
            var modifier = (T1) Dic[typeof (T1)].Dequeue();
            modifier.Builder = builder;
            return modifier;
        }
        Dic.Add(typeof (T1), new Queue<Modifier<T>>());
        var obj1 = new T1 {
            Builder = builder
        };
        obj1.Release = () => Dic[typeof (T1)].Enqueue(obj1);
        return obj1;
    }
}