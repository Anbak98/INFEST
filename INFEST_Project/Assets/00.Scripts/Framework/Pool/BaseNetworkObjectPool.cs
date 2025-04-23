using Fusion;

public struct NetworkPoolObject
{
    public bool IsPooled;
    public int index;
    public NetworkObject obj;
}

public class BaseNetworkObjectPool
{
    public BaseNetworkObjectPool(int poolSize)
    {
        _pool = new NetworkPoolObject[poolSize];
    }

    public NetworkPoolObject Get()
    {
        return GetObjWithSwap();
    }

    public void Add(NetworkObject newObj)
    {
        NetworkPoolObject newPool = new NetworkPoolObject()
        {
            IsPooled = false,
            index = _numObject,
            obj = newObj
        };

        _pool[_numObject] = newPool;

        ++_numObject;
        ++_numNotPooled;
    }

    public void Return(NetworkPoolObject pooledObj)
    {
        ReturnObjWithSwap(pooledObj);
    }

    #region Private
    private readonly NetworkPoolObject[] _pool;

    private int _numObject = 0;
    private int _numPooled = 0;
    private int _numNotPooled = 0; 

    private NetworkPoolObject GetObjWithSwap()
    {
        NetworkPoolObject poolObj = _pool[0];

        _pool[0] = _pool[_numNotPooled - 1];
        _pool[0].index = 0;

        _pool[_numNotPooled - 1] = poolObj;

        poolObj.IsPooled = true;
        poolObj.index = _numNotPooled - 1;

        _numPooled++;
        _numNotPooled--;

        return poolObj;
    }

    private void ReturnObjWithSwap(NetworkPoolObject pooledObj)
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
