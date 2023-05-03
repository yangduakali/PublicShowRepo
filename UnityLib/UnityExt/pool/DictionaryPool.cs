using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityExt.pool;

public class DictionaryPool<T1, T2> where T2 : IPoolObject where T1 : notnull{
    private readonly Dictionary<T1, Pool<T2>> _dicPool = new ();
    private readonly DictionaryPoolCreateAction<T2, T1> _create;
    private readonly DictionaryPoolCreateActionAsync<T2, T1> _createAsync;

    public DictionaryPool(DictionaryPoolCreateAction<T2, T1> create)
    {
        _create = create;
        _createAsync =  null;
    }
    public DictionaryPool(DictionaryPoolCreateActionAsync<T2, T1> create)
    {
        _create =  null;
        _createAsync = create;
    }
    public T2 Get(T1 key)
    {
        if (_dicPool.ContainsKey(key))
            return _dicPool[key].Get();
        _dicPool.Add(key, new Pool<T2>( (() => _create(key))));
        return _dicPool[key].Get();
    }

    public Task<T2> GetAsync(T1 key)
    {
        if (_dicPool.ContainsKey(key))
            return _dicPool[key].GetAsync();
        _dicPool.Add(key, new Pool<T2>((() => _createAsync(key))));
        return _dicPool[key].GetAsync();
    }

    public void Clear() => this._dicPool.Clear();
}