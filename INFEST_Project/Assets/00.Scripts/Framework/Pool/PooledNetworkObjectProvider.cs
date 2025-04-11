using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PooledNetworkObjectProvider : NetworkObjectProviderDefault
{
    protected override NetworkObject InstantiatePrefab(NetworkRunner runner, NetworkObject prefab)
    {
        return base.InstantiatePrefab(runner, prefab);
    }

    protected override void DestroyPrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId, NetworkObject instance)
    {
    }
}
