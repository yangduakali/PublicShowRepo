using UnityEngine;
// ReSharper disable InconsistentNaming

namespace UnityExt.value;

public class ValueTSO<T> : ScriptableObject
{
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnTValue<T> onValueChange = OnValueChange;
            if (onValueChange == null)
                return;
            onValueChange(value);
        }
    }

    public event OnTValue<T> OnValueChange;
}