using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityExt.pool;

public class Pool<T> where T : IPoolObject{

    public Pool(PoolCreateAction<T> create)
    {
        _create = create;
        _createAsync = null;
    }

    public Pool(PoolCreateActionAsync<T> create)
    {
        _createAsync = create;
        _create = null;

    }
    
    private readonly PoolCreateAction<T> _create;
    private readonly PoolCreateActionAsync<T> _createAsync;
    private readonly Queue<T> _pool = new Queue<T>();

    public int Count => _pool.Count;
    public T Get()
    {
        if (_pool.Count == 0)
        {
            var n = _create();
            n.Release = () => {
                _pool.Enqueue(n);
                n.OnRelease();
            };
            n.OnPoolRequested();
            return n;
        }
        var obj1 = _pool.Dequeue();
        obj1.OnPoolRequested();
        return obj1;
    }
    public async Task<T> GetAsync()
    {
        if (_pool.Count == 0) {

            var n = await _createAsync();
            n.Release = () => {
                _pool.Enqueue(n);
                n.OnRelease();
            };
            n.OnPoolRequested();
            return n;
        }
        var dequeue = _pool.Dequeue();
        dequeue.OnPoolRequested();
        return dequeue;
    }
    public void Clear() => _pool.Clear();
}