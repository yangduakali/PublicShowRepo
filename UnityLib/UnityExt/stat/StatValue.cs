namespace UnityExt.stat;

internal class StatValue
{
    public StatValue MaxValue;
    public StatValue ChildValue;
    protected float CurrentValue;

    public float Value
    {
        get => CurrentValue;
        set
        {
            float num = CurrentValue;
            if (MaxValue != null && value > MaxValue.Value) value = MaxValue.Value;
            CurrentValue = value;
            if (ChildValue != null && ChildValue.Value >  value) ChildValue.CurrentValue = value;
            OnValueChange?.Invoke(CurrentValue);
            OnDeltaValueChange?.Invoke(CurrentValue - num);
        }
    }

    public event OnFloat OnValueChange;
    public event OnFloat OnDeltaValueChange;

    public void ForceValue(float value)
    {
        CurrentValue = value;
    }

    public void Clear()
    {
        CurrentValue = 0.0f;
        OnValueChange = null;
        OnDeltaValueChange = null;
        MaxValue = null;
        ChildValue = null;
    }
}