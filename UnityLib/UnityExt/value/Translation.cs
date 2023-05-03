using System;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace UnityExt.value;

[Serializable]
public struct Translation
{
    public Matrix4x4 matrix;

    public static Translation Zero => new Translation()
    {
        matrix = Matrix4x4.identity
    };

    public Vector3 Position
    {
        get => matrix.GetPosition();
        set => matrix.SetTRS(value, matrix.rotation, matrix.lossyScale);
    }

    public float PositionX
    {
        get => Position.x;
        set
        {
            var position = Position;
            Position = new Vector3(value, position.y, position.z);
        }
    }

    public float PositionY
    {
        get => Position.y;
        set
        {
            var position = Position;
            Position = new Vector3(position.x, value, position.z);
        }
    }

    public float PositionZ
    {
        get => Position.y;
        set
        {
            var position = Position;
            Position = new Vector3(position.x, position.y, value);
        }
    }

    public Quaternion Rotation
    {
        get => matrix.rotation;
        set => matrix.SetTRS(matrix.GetPosition(), value, matrix.lossyScale);
    }

    public Vector3 Euler
    {
        get => Rotation.eulerAngles;
        set => Rotation = Quaternion.Euler(value);
    }

    public float EulerX
    {
        get => Rotation.eulerAngles.x;
        set
        {
            var euler = Euler;
            Rotation = Quaternion.Euler(value, euler.y, euler.z);
        }
    }

    public float EulerY
    {
        get => Rotation.eulerAngles.y;
        set
        {
            var euler = Euler;
            Rotation = Quaternion.Euler(euler.x, value, euler.z);
        }
    }

    public float EulerZ
    {
        get => Rotation.eulerAngles.y;
        set
        {
            var euler = Euler;
            Rotation = Quaternion.Euler(euler.x, euler.y, value);
        }
    }

    public Vector3 Scale
    {
        get => matrix.lossyScale;
        set => matrix.SetTRS(matrix.GetPosition(), matrix.rotation, value);
    }

    public float ScaleX
    {
        get => Scale.x;
        set
        {
            var scale = Scale;
            Scale = new Vector3(value, scale.y, scale.z);
        }
    }

    public float ScaleY
    {
        get => Scale.y;
        set
        {
            var scale = Scale;
            Scale = new Vector3(scale.x, value, scale.z);
        }
    }

    public float ScaleZ
    {
        get => Scale.y;
        set
        {
            var scale = Scale;
            Scale = new Vector3(scale.x, scale.y, value);
        }
    }

    public Vector3 right => new Vector3(matrix.m00, matrix.m10, matrix.m20);

    public Vector3 up => new Vector3(matrix.m01, matrix.m11, matrix.m21);

    public void SetUniformScale(float value) => Scale = Vector3.one * value;

    public void SetPositionAndRotation(Vector3 position, Quaternion rotation) => matrix.SetTRS(position, rotation, matrix.lossyScale);

    public void TRS(Vector3 position, Quaternion rotation, Vector3 scale) => matrix.SetTRS(position, rotation, scale);
}