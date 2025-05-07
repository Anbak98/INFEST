using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : NetworkBehaviour
{
    // TickTimer timer; // 나중에 타이머로 경보기하면 될듯
    private readonly int SpawnMonsterNumberOnEachWave = 50;
    [SerializeField] private List<Transform> points;
    [SerializeField] private NetworkPrefabRef MonsterPrefab;

    public void SpawnMonsterOnWave(Transform waveTarget)
    {
        if (Runner.IsServer)
        {
            int remainSpawnNumber = SpawnMonsterNumberOnEachWave;
            int iteral = 20;
            while (remainSpawnNumber > 0 && iteral > 0)
            {
                int point = Random.Range(0, points.Count);
                int num = Random.Range(0, 8);
                num = num > remainSpawnNumber ? remainSpawnNumber : num;
                Spawn(points[point].position, num, waveTarget);
                remainSpawnNumber -= num;
                iteral--;
            }
        }
    }

    private void Spawn(Vector3 position, int num, Transform waveTarget)
    {
        for (int i = 0; i < num; i++)
        {
            MonsterNetworkBehaviour mnb = Runner.Spawn(MonsterPrefab, position).GetComponent<MonsterNetworkBehaviour>();
            mnb.GetComponent<NavMeshAgent>().enabled = true;
            mnb.target = waveTarget;
            mnb.FSM.ChangePhase<PJ_HI_Phase_Wave>();
        }
    }
}