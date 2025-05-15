using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnhancedMonsterSpawner : NetworkBehaviour
{
    [SerializeField] private LayerMask spawnPointLayerMask;
    [SerializeField] private MonsterScriptableObject MonsterMap;

    private List<UnityEngine.Collider> spawnPoints = new();
    private TickTimer spawnDelayTimer;

    private Queue<int> monsterSpawnQueue = new();
    private Transform target;

    public override void Spawned()
    {
        base.Spawned();
        MonsterMap.Init();
    }

    public override void FixedUpdateNetwork()
    {
        if (spawnDelayTimer.ExpiredOrNotRunning(Runner))
        {
            if (monsterSpawnQueue.Count > 0 && spawnPoints.Count > 0)
            {
                int rand = Random.Range(0, spawnPoints.Count);
                NetworkObject monster = Runner.Spawn(MonsterMap.GetByKey(monsterSpawnQueue.Dequeue()), spawnPoints[rand].transform.position);
                MonsterNetworkBehaviour mnb = monster.GetComponent<MonsterNetworkBehaviour>();

                mnb.OnWave(target);

                spawnDelayTimer = TickTimer.CreateFromSeconds(Runner, 0.1f);
            }
        }
    }

    public void CallWave(Transform from)
    {
        int distance = 15;
        int iteral = 5;

        do
        {
            spawnPoints = Physics.OverlapSphere(transform.position, distance, spawnPointLayerMask).ToList();
            distance *= 2;
            iteral--;
        } while (spawnPoints.Count == 0 && iteral > 0);

        if (spawnPoints.Count > 0)
        {
            monsterSpawnQueue = new();

            for (int i = 0; i < 35; i++)
            {
                monsterSpawnQueue.Enqueue(1001);
            }

            for (int i = 0; i < 14; i++)
            {
                monsterSpawnQueue.Enqueue(1002);
            }

            //for (int i = 0; i < 1; i++)
            //{
            //    monsterSpawnQueue.Enqueue(2001);
            //}

            target = from;
        }
    }
}