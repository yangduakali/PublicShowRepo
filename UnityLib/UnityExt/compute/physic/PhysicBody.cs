using UnityEngine;
using UnityExt.attributes;
// ReSharper disable Unity.InefficientPropertyAccess

namespace UnityExt.compute.physic;
[AddComponentMenu("Compute/Physic/Physic Body")]
[ClassIcon("physic_body")]
public class PhysicBody : MonoBehaviour{
    public event PhysicDataUpdated OnBodyUpdated = delegate { };
    public event PhysicDataCollisionEnter OnBodyCollisionEnter = delegate { };

    public float Mass {
        get => mass;
        set
        {
            mass = value;
            ReCalculatePhysic();
        } 
    }
    public float LinearDrag {
        get => linearDrag;
        set
        {
            linearDrag = value;
            ReCalculatePhysic();
        }
    }
    public float AngularDrag {
        get => angularDrag;
        set
        {
            angularDrag = value;
            ReCalculatePhysic();
        }
    }
    public Vector2 Velocity {
        get => Physic.GetVelocity(_physicId);
        set => Physic.SetVelocity(_physicId, value);
    }
    public float Rotation {
        get => Physic.GetRotation(_physicId);
        set => Physic.SetRotation(_physicId, value);
    }
   
    [SerializeField] private int layer;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private float mass = 1;
    [SerializeField] private float gravityScale = 1;
    [SerializeField] private float linearDrag = 0.1f;
    [SerializeField] private float angularDrag = 0.1f;

    [Space(30)] 
    [ReadOnly] 
    [SerializeField] private Vector2 linearVelocity;
    [ReadOnly] 
    [SerializeField] private float angularVelocity;
    
    private int _physicId;
    private Transform _transform;

    private void Awake(){
        _transform = transform;
    }

    private void OnEnable(){
        _physicId = Physic.AddData(new PhysicData {
            Layer = layer,
            Radius = radius,
            Mass = mass,
            LinearDrag = linearDrag,
            AngularDrag = angularDrag,
            GravityScale = gravityScale,
            Position = _transform.position,
            Rotation = _transform.rotation.eulerAngles.z,
        }, new PhysicProperty {
            OnUpdated = OnUpdate,
            OnCollisionEnter = OnBodyCollisionEnterInvoke,
            Transform = transform
        });

    }

    private void OnBodyCollisionEnterInvoke(PhysicData body, PhysicData other){
        OnBodyCollisionEnter?.Invoke(body,other);
    }

    private void OnUpdate(PhysicData body){
        _transform.position = body.Position;
        _transform.rotation = Quaternion.Euler(0,0,body.Rotation);
        linearVelocity = body.LinearVelocity;
        angularVelocity = body.AngularVelocity;
        OnBodyUpdated?.Invoke(body);
    }
    private void OnValidate(){
        if (mass < 0.01f) mass = 0.01f;
        if (angularDrag < 0) angularDrag = 0;
        if (linearDrag < 0) linearDrag = 0;
        if (radius < 0) radius = 0;
    }
    public void AddForce(Vector2 force){
        Physic.AddForce(_physicId, force);
    }
    public void AddTorque(float torque){
        Physic.AddTorque(_physicId, torque);
    }
    private void ReCalculatePhysic(){ }

    private void RemovePhysicData(){
        if(!Physic.IsValid()) return;
        Physic.RemoveData(_physicId);
    }
    
    private void OnDisable(){
        RemovePhysicData();
    }

    private void OnDestroy(){
        RemovePhysicData();
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}