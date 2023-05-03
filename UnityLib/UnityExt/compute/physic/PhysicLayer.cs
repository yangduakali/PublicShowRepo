using UnityEngine;

namespace UnityExt.compute.physic;

[CreateAssetMenu]
public class PhysicLayer : ScriptableObject{
    [SerializeField] 
    internal string[] layerName;
    public int[] layerValue;
}