using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace UnityExt.stat;

public abstract class Modifier<T> where T : Enum
{
    internal Action Release;

    public StatBuilder<T> Builder { get; internal set; }

    public virtual void OnTick(float deltaTime)
    {
    }

    public virtual void OnStatGetValue(T type, ref float value)
    {
    }

    public virtual void OnStatDeltaChange(T type, ref float value)
    {
    }

    public virtual void OnStack<T1>(ref T1 item) where T1 : Modifier<T>, new()
    {
    }

    public virtual void OnOtherModifierAdded<T1>(ref T1 item) where T1 : Modifier<T>, new()
    {
    }

    public virtual void OnOtherModifierRemoved<T1>(ref T1 item) where T1 : Modifier<T>
    {
    }

    public virtual void OnAdded()
    {
    }

    public virtual void OnRemove()
    {
    }
}