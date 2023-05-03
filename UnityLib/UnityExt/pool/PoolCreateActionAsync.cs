using System.Threading.Tasks;

namespace UnityExt.pool;
public delegate Task<T> PoolCreateActionAsync<T>() where T : IPoolObject;