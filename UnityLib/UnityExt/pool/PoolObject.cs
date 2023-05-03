using UnityEngine;

namespace UnityExt.pool;

public class PoolObject : MonoBehaviour, IPoolObject
{
    public PoolRelease Release { get;  set; }
    public virtual void OnPoolRequested() => this.gameObject.SetActive(true);
    public virtual void OnRelease() => this.gameObject.SetActive(false);
}