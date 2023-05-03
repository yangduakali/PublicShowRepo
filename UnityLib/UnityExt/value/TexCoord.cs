using UnityEngine;

namespace UnityExt.value;

[System.Serializable]
public class TexCoord{
    public Vector2 cord;
    public string assetGuid;
    
    public static implicit operator Vector4(TexCoord v) {
        return new Vector4(v.cord.x,v.cord.y ,0,0);
    }
    public static implicit operator Vector2Int(TexCoord v) {
        return new Vector2Int((int)v.cord.x ,(int)v.cord.y);
    }
}