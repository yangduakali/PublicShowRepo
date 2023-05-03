using System;
using UnityEngine.VFX;
using UnityExt.pool;

namespace UnityEditorExt.vfx;

internal class EffectEntity : VFXSpawnerCallbacks, IPoolObject{
    public PoolRelease Release { get; set; }
    internal VisualEffect VisualEffect;

    public void Set(VisualEffectAsset asset){
       
    }

    public override void OnPlay(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent){
    }

    public override void OnUpdate(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent){
    }

    public override void OnStop(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent){
    }

    public void OnPoolRequested(){
        
    }

    public void OnRelease(){
        
        VisualEffect.visualEffectAsset = null;
    }
}