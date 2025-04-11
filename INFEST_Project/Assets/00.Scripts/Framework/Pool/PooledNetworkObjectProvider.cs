using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PooledNetworkObjectProvider : NetworkObjectProviderDefault
{
    private Dictionary<NetworkPrefabId, BaseNetworkObjectPool<NetworkObject>> _pools = new();

    public void RegisterPool(NetworkPrefabId prefabId, BaseNetworkObjectPool<NetworkObject> pool)
    {
        _pools[prefabId] = pool;
    }

    protected override NetworkObject InstantiatePrefab(NetworkRunner runner, NetworkObject prefab)
    {
        var prefabId = prefab.;

        if (_pools.TryGetValue(prefabId, out var pool))
        {
            var pooled = pool.Get();
            return pooled.obj;
        }

        // fallback to default behavior
        return base.InstantiatePrefab(runner, prefab);
    }

    protected override void DestroyPrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId, NetworkObject instance)
    {
        if (_pools.TryGetValue(prefabId, out var pool))
        {
            pool.Return(new NetworkPoolObject<NetworkObject>
            {
                obj = instance,
                IsPooled = true,
                index = -1
            });
        }
        else
        {
            base.DestroyPrefabInstance(runner, prefabId, instance);
        }
    }
}
