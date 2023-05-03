// ReSharper disable InconsistentNaming
namespace UnityExt.value;

public struct Vector2uint{
    public uint x;
    public uint y;
    
    public static implicit operator (uint,uint)(Vector2uint v){
        return (v.x, v.y);
    }
    
    public static implicit operator Vector2uint((uint,uint) v){
        var result = new Vector2uint {
            x = v.Item1,
            y = v.Item2
        };
        return result;
    }
}