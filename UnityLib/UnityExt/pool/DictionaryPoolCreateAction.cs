namespace UnityExt.pool;

public delegate T DictionaryPoolCreateAction<out T, in T2>(T2 key) where T : IPoolObject;