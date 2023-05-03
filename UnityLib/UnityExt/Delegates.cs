using UnityEngine;

namespace UnityExt;

public delegate void OnFloat(float value);
public delegate void OnInt(int value);
public delegate void OnRefFloat(ref float value);
public delegate void OnTransform(Transform value);
public delegate void OnTValue<in T>(T value);