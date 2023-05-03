using System;
using System.Collections.Generic;

namespace UnityExt.stat;

public class StatBuilder<T> where T : Enum {
    private readonly Dictionary<T, StatValue> _stats = new();
    private readonly List<Modifier<T>> _modifiers = new();
    public StatBuilder()
    {
      int length = Enum.GetValues(typeof (T)).Length;
      for (int index = 0; index < length; ++index)
        _stats.Add((T)Enum.GetValues(typeof (T)).GetValue(index), new StatValue());
    }
    private event Action<Modifier<T>> OnModifierAdd  = delegate {  };
    private event Action<Modifier<T>> OnModifierRemove = delegate{  };
    
    public float GetValue(T type, bool includeModifier = true)
    {
      float num = _stats[type].Value;
      if (!includeModifier)
        return num;
      for (int index = 0; index < _modifiers.Count; ++index)
        _modifiers[index].OnStatGetValue(type, ref num);
      return num;
    }
    public void SetDelta(T type, float deltaValue)
    {
      _modifiers.ForEach(x => x.OnStatDeltaChange(type, ref deltaValue));
      _stats[type].Value += deltaValue;
    }
    public void ForceSet(T type, float value) => _stats[type].ForceValue(value);
    public void Clear(bool clearEvents = false)
    {
      foreach (KeyValuePair<T, StatValue> stat in _stats)
      {
        if (clearEvents)
          stat.Value.Clear();
        else
          stat.Value.ForceValue(0.0f);
      }
    }
    public void Update(float deltaTime)
    {
      for (int index = 0; index < _modifiers.Count; ++index)
        _modifiers[index].OnTick(deltaTime);
    }

    public float RegisterStatOnValueChange(T type, OnFloat function)
    {
      _stats[type].OnValueChange += function;
      return _stats[type].Value;
    }
    public void RegisterStatOnDeltaValueChange(T type, OnFloat function){
      _stats[type].OnDeltaValueChange += function;
    }
    public void UnregisterStatOnValueChange(T type, OnFloat function){
      _stats[type].OnValueChange -= function;
    }
    public void UnregisterStatOnDeltaValueChange(T type, OnFloat function){
      _stats[type].OnDeltaValueChange -= function;
    }

    public void RegisterMaxValue(T type, T typeMax)
    {
      _stats[type].MaxValue = _stats[typeMax];
      _stats[typeMax].ChildValue = _stats[type];
    }
    
    public TS AddModifier<TS>() where TS : Modifier<T>, new()
    {
      var modifier1 = StatUltis<T>.GetModifier<TS>(this);
      
      for (int index = 0; index < _modifiers.Count; ++index)
      {
        if (_modifiers[index].GetType() != modifier1.GetType())
          _modifiers[index].OnOtherModifierAdded<TS>(ref modifier1);
      }
      for (int index = 0; index < _modifiers.Count; ++index)
      {
        Modifier<T> modifier2 = _modifiers[index];
        if (modifier2.GetType() != modifier1.GetType()) continue;
        modifier2.OnStack<TS>(ref modifier1);
        modifier2.Release();
        OnModifierAdd(modifier2);
        return (TS) _modifiers[index];
      }
      _modifiers.Add(modifier1);
      OnModifierAdd(modifier1);
      modifier1.OnAdded();
      foreach (KeyValuePair<T, StatValue> stat in _stats)
        stat.Value.Value = stat.Value.Value;
      return modifier1;
    }
    
    public void RemoveModifier<TS>() where TS : Modifier<T>, new()
    {
      Modifier<T> modifier = null;
      for (int index = 0; index < _modifiers.Count; ++index) {
        if (_modifiers[index].GetType() == typeof(TS)) continue;
        modifier = _modifiers[index];
        break;
      }
      if (modifier == null) return;
      for (int index = 0; index < _modifiers.Count; ++index)
      {
        if (_modifiers[index].GetType() != typeof (TS))
          _modifiers[index].OnOtherModifierRemoved(ref modifier);
      }
      if (modifier == null)
        return;
      modifier.OnRemove();
      _modifiers.Remove(modifier);
      modifier.Release();
      OnModifierRemove(modifier);
      foreach (KeyValuePair<T, StatValue> stat in _stats)
        stat.Value.Value = stat.Value.Value;
    }
    public bool TryGetModifier<TS>(out TS modifier) where TS : Modifier<T>, new()
    {
      for (int index = 0; index < _modifiers.Count; ++index) {
        if (_modifiers[index].GetType() != typeof(TS)) continue;
        modifier = (TS) _modifiers[index];
        return true;
      }
      modifier = default;
      return false;
    }
    public void RegisterOnModifierAdded(Action<Modifier<T>> function){
      OnModifierAdd += function;
    }
    public void RegisterOnModifierRemoved(Action<Modifier<T>> function){
      OnModifierRemove -= function;
    }
  }