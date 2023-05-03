using System;
using UnityEngine;

namespace UnityExt.game.gameobject;

public class Actor : MonoBehaviour{
    protected Transform Transform;
    
    protected void Awake(){
        Transform = transform;
    }
}