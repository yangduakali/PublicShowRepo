namespace UnityExt.pool;

public delegate T PoolCreateAction<out T>() where T : IPoolObject;