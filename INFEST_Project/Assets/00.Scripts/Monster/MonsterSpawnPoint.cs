using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawnPoint : NetworkBehaviour
{
    private MonsterSpawner Spawner;
    public NetworkPrefabRef Monster;
    public bool IsActivated = true;

    public void Init(MonsterSpawner spawner)
    {
        Spawner = spawner;
    }

    Player[] players;
    public void Spawn(int num)
    {
        if (Runner.IsServer)
        {
            if (players == null)
            {
                players = FindObjectsOfType<Player>();
            }

            for (int i = 0; i < num; i++)
            {
                MonsterNetworkBehaviour mnb = Runner.Spawn(Monster, transform.position).GetComponent<MonsterNetworkBehaviour>();
                mnb.GetComponent<NavMeshAgent>().enabled = true;
                mnb.target = players[Random.Range(0, players.Length)].transform;
            }
        }
    }
}