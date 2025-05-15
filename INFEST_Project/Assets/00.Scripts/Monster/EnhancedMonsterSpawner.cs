using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnhancedMonsterSpawner : NetworkBehaviour
{
    [SerializeField] private LayerMask waveTriggerLayer;
    [SerializeField] private MonsterScriptableObject MonsterMap;

    private UnityEngine.Collider[] spawnPoints;
    private TickTimer spawnDelayTimer;

    private Queue<int> monsterSpawnQueue;
    private Transform target;

    public override void FixedUpdateNetwork()
    {
        if (spawnDelayTimer.ExpiredOrNotRunning(Runner))
        {
            if (monsterSpawnQueue.Count > 0)
            {
                int rand = Random.Range(0, spawnPoints.Length);
                NetworkObject monster = Runner.Spawn(MonsterMap.GetByKey(monsterSpawnQueue.Dequeue()), spawnPoints[rand].transform.position);
                MonsterNetworkBehaviour mnb = monster.GetComponent<MonsterNetworkBehaviour>();

                mnb.target = target;
                mnb.OnWave();

                spawnDelayTimer = TickTimer.CreateFromSeconds(Runner, 0.025f);
            }
        }
    }

    public void CallWave(Transform from)
    {
        int distance = 15;
        int iteral = 5;

        do
        {
            spawnPoints = Physics.OverlapSphere(transform.position, distance, waveTriggerLayer);
            distance *= 2;
            iteral--;
        } while (spawnPoints == null && iteral > 0);

        if (spawnPoints != null)
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

            for (int i = 0; i < 1; i++)
            {
                monsterSpawnQueue.Enqueue(2001);
            }

            target = from;
        }
    }
}