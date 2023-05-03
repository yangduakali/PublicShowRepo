
using UnityEngine;

namespace UnityExt;

public static class Vector3Ext{
    
    public static Vector3 DirectionTo(this Vector3 from, Vector3 to) => to - from;
    public static Vector3 WithX(this Vector3 vector, float x)
    {
        vector.x = x;
        return vector;
    }
    public static Vector3 WithY(this Vector3 vector, float y)
    {
        vector.y = y;
        return vector;
    }
    public static Vector3 WithZ(this Vector3 vector, float z)
    {
        vector.z = z;
        return vector;
    }
    public static Vector3 WithOffsetX(this Vector3 vector, float x)
    {
        vector.x += x;
        return vector;
    }
    public static Vector3 WithOffsetY(this Vector3 vector, float y)
    {
        vector.y += y;
        return vector;
    }
    public static Vector3 WithOffsetZ(this Vector3 vector, float z)
    {
        vector.z += z;
        return vector;
    }
    public static Vector3 WithOffsetXY(this Vector3 vector, float x, float y)
    {
        vector.x += x;
        vector.y += y;
        return vector;
    }
    public static Vector3 WithOffset(this Vector3 vector, float x, float y, float z)
    {
        vector.x += x;
        vector.y += y;
        vector.z += z;
        return vector;
    }
    public static Vector3 WithOffset(this Vector3 vector, Vector3 offset)
    {
        vector.x += offset.x;
        vector.y += offset.y;
        vector.z += offset.z;
        return vector;
    }

}