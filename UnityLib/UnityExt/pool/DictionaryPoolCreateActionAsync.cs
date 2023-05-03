using System.Threading.Tasks;

namespace UnityExt.pool;

public delegate Task<T> DictionaryPoolCreateActionAsync<T, in T2>(T2 key) where T : IPoolObject;