using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : NetworkBehaviour
{
    public MonsterSpawner Spawner;
    public NetworkObject Monster;
    public bool IsActivated = true;

    public void Init(MonsterSpawner spawner)
    {
        Spawner = spawner;
    }

    public void Spawn(int num)
    {
        if(Runner.IsServer)
        {
            for (int i = 0; i < num; i++)
            {
                //Runner.Spawn(Monster, transform.position);
            }
        }
    }
}