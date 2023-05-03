using System;
using UnityEngine;

namespace UnityExt.value;

[Serializable]
public struct MinMaxCurveFloat
{
    public Mode mode;
    public float constant;
    public float min;
    public float max;
    public AnimationCurve constantCurve;
    public AnimationCurve minCurve;
    public AnimationCurve maxCurve;
    public float curvePower;

    public static MinMaxCurveFloat One => new()
    {
        constant = 1f,
        curvePower = 1f
    };

    public float Value(float t){
        return mode switch {
            Mode.Constant => constant,
            Mode.Random => UnityEngine.Random.Range(min, max),
            Mode.Curve => constantCurve.Evaluate(t) * curvePower,
            Mode.RandomCurve => UnityEngine.Random.Range(minCurve.Evaluate(t), maxCurve.Evaluate(t)) * curvePower,
            _ => 0.0f
        };
    }

    public enum Mode
    {
        Constant,
        Random,
        Curve,
        RandomCurve,
    }
}