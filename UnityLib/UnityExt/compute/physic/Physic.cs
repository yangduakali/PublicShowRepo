using UnityEngine;

namespace UnityExt.compute.physic;

public class Physic : ComputeSystem<PhysicData,PhysicProperty>{
    
    public Physic(ComputeShader computeShader, PhysicLayer layer) : base(computeShader){
        if (_instance != null) {
            Debug.LogError($"{this} already created. Multiple {this} not allowed.");
            return;
        }

        _layer = layer;
        _instance = this;
    }
    private static Physic _instance;
    private PhysicLayer _layer;
    public override void Dispatch(){
        if(DataLenght == 0) return;
        Buffer.SetData(Data);
        ComputeShader.SetFloat("delta_time",Time.deltaTime);
        ComputeShader.SetInts("layer_data", _layer.layerValue);
        ComputeShader.SetBuffer(0,"physic_data",Buffer);
        ComputeShader.Dispatch(0,1,1,1);

        Buffer.GetData(Data);
        for (int i = 0; i < DataLenght; i++) {
            var body = Data[i];
            if(!body.IsActive) continue;
            
            var property = Properties[i];
            property.OnUpdated?.Invoke(body);

            if (body.IsCollide == 0) {
                Properties[i].LastCollideWithId = -1;    
                continue;
            }

            if (property.LastCollideWithId == body.CollideWithID) {
                continue;                
            }

            Properties[i].LastCollideWithId = body.CollideWithID;
            Properties[i].OnCollisionEnter?.Invoke(body,Data[body.CollideWithID]);
            Properties[body.CollideWithID].OnCollisionEnter?.Invoke(Data[body.CollideWithID],body);
        }
    }
    
    
    protected override PhysicData InitializeData(PhysicData data, int index){
        if (data.Mass < 0.01f) data.Mass = 0.01f;
        if (data.AngularDrag < 0) data.AngularDrag = 0f;
        if (data.LinearDrag < 0) data.LinearDrag = 0f;
        if (data.Radius < 0) data.LinearDrag = 0f;
        data.Id = index;
        return base.InitializeData(data, index);
    }
    
    protected override void RemoveDataCompute(int index){
        Properties[index].LastCollideWithId = -1;
        Properties[index].Transform = null;
        base.RemoveDataCompute(index);
    }
    public static int AddData(PhysicData data, PhysicProperty property){
        return _instance.AddComputeData(data, property);
    }
    
    public static void RemoveData(int index){
        _instance.RemoveDataCompute(index);
    }
    
    public static void SetVelocity(int id, Vector2 velocity){
        _instance.Data[id].LinearVelocity = velocity;
    }
    
    public static Vector2 GetVelocity(int id){
        return _instance.Data[id].LinearVelocity;
    }
    
    public static void SetRotation(int id, float rotation){
        _instance.Data[id].Rotation = rotation;
    }
    
    public static float GetRotation(int id){
        return _instance.Data[id].Rotation;
    }

    public static void SetPosition(int id, Vector2 position){
        _instance.Data[id].Position = position;
    }
    
    public static void SetRadius(int id, float radius){
        _instance.Data[id].Radius = radius;
    }

    
    public static void AddForce(int id, Vector2 force){
        _instance.Data[id].LinearForce += force * Time.deltaTime;
    }
    
    public static void AddTorque(int id, float torque){
        _instance.Data[id].AngularForce += torque * Time.deltaTime;
    }
    
    public override void Release(){
        base.Release();
        _instance = null;
    }
    
    internal static Transform GetTransform(int id){
        return _instance.Properties[id].Transform;
    }

    public static bool IsValid() =>  _instance != null;

    public static void SetLayer(PhysicLayer layer){
        _instance._layer = layer;
    }
}

public delegate void PhysicDataUpdated( PhysicData body);
public delegate void PhysicDataCollisionEnter(PhysicData body, PhysicData other);
