namespace UnityExt.pool;

public interface IPoolObject
{
    PoolRelease Release { get; set; }
    void OnPoolRequested();
    void OnRelease();
}