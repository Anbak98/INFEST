using Fusion;

public struct NetworkPoolObject<T> where T : NetworkObject
{
    public bool IsPooled;
    public int index;
    public T obj;
}

public class BaseNetworkObjectPool<T> where T : NetworkObject
{
    public BaseNetworkObjectPool(int poolSize)
    {
        _pool = new NetworkPoolObject<T>[poolSize];
    }

    public NetworkPoolObject<T> Get()
    {
        if(_numNotPooled < 1)
        {
            CrateNewObjectToPool();
        }

        return GetObjWithSwap();
    }

    public void Return(NetworkPoolObject<T> pooledObj)
    {
        ReturnObjWithSwap(pooledObj);
    }

    #region Private
    private readonly NetworkPoolObject<T>[] _pool;

    private int _numObject;
    private int _numPooled;
    private int _numNotPooled;

    private void CrateNewObjectToPool()
    {
        if (_numObject < _pool.Length)
        {
            
        }

        _numNotPooled++;
    }

    private NetworkPoolObject<T> GetObjWithSwap()
    {
        NetworkPoolObject<T> poolObj = _pool[0];

        _pool[0] = _pool[_numNotPooled - 1];
        _pool[0].index = 0;

        _pool[_numNotPooled - 1] = poolObj;

        poolObj.IsPooled = true;
        poolObj.index = _numNotPooled - 1;

        _numPooled++;
        _numNotPooled--;

        return poolObj;
    }

    private void ReturnObjWithSwap(NetworkPoolObject<T> pooledObj)
    {
        _pool[pooledObj.index] = _pool[_numNotPooled];
        _pool[pooledObj.index].index = pooledObj.index;
        _pool[_numNotPooled] = pooledObj;
        _pool[_numNotPooled].index = pooledObj.index;

        _numPooled--;
        _numNotPooled++;
    }
    #endregion
}
