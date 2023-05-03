using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace UnityExt.compute.physic;

public struct PhysicData : IDataCompute{
    public bool IsActive {
        get => _isActive == 1; 
        set => _isActive = value ? 1 : 0;
    }
    public int Id;
    public int Layer;
    private int _isActive;
    public float Radius;
    public float Mass;
    public float GravityScale;
    public float LinearDrag;
    public float AngularDrag;

    public Vector2 Position;
    public Vector2 LinearVelocity;
    public Vector2 LinearForce;
    
    public float Rotation;
    public float AngularVelocity;
    public float AngularForce;
    
    internal int IsCollide;
    internal int CollideWithID;

    public bool TryGetComponent<T>(out T component) where T : Component{
        component = null;
        var transform = Physic.GetTransform(Id);
        return transform != null && transform.TryGetComponent(out component);
    }
}

public struct PhysicProperty : IDataComputeProperty{
    public int ID {
        get => _id; 
        set => _id = value;
    }
    private int _id;
    public PhysicDataUpdated OnUpdated;
    public PhysicDataCollisionEnter OnCollisionEnter;
    public Transform Transform;
    internal int LastCollideWithId;
    
}