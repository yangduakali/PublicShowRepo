using System;
using System.Collections.Generic;
namespace UnityExt;
public static class CollectionExt{
    public static T PickRandom<T>(this IList<T> enumerator){
      return enumerator[UnityEngine.Random.Range(0, enumerator.Count)];
    }
    public static void Foreach<T>(this T[] array, Action<T> action)
    {
      int length = array.Length;
      for (int index = 0; index < length; ++index)
        action(array[index]);
    }
    public static void AddUnique<T>(this List<T> list, T item)
    {
      if (list.Contains(item))
        return;
      list.Add(item);
    }
    public static void Shuffle<T>(this List<T> list){
      list.Sort(((a, b) => 1 - 2 * UnityEngine.Random.Range(0, list.Count)));
    }
    public static void EnqueueUnique<T>(this Queue<T> q, T item)
    {
      if (q.Contains(item))
        return;
      q.Enqueue(item);
    }
    public static void Iterate(this int count, Action<int> action)
    {
      for (int index = 0; index < count; ++index)
        action(index);
    }
    public static bool TryRemove<T>(this List<T> list, T item)
    {
      if (!list.Contains(item))
        return false;
      list.Remove(item);
      return true;
    }
    public static void RemoveFor<T>(this List<T> list, Func<T, bool> func)
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < list.Count; ++index)
      {
        if (func(list[index]))
          intList.Add(index);
      }
      for (int index = 0; index < intList.Count; ++index)
        list.RemoveAt(intList[index]);
    }
    public static T[] Pick<T>(this T[] array, Func<T, bool> func)
    {
      List<T> objList = new List<T>();
      for (int index = 0; index < array.Length; ++index)
      {
        if (func(array[index]))
          objList.Add(array[index]);
      }
      return objList.ToArray();
    }
}