using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterSpawner : NetworkBehaviour
{
    // TickTimer timer; // 나중에 타이머로 경보기하면 될듯
    public int SpawnMonsterNumberOnEachWave = 5;
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

                for (int i = 0; i < num; i++)
                {
                    Vector3 offset = Random.insideUnitSphere * 1f;
                    Vector3 spawnPos = points[point].position + offset;
                    offset.y = 0f;

                    NetworkObject networkObj = Runner.Spawn(MonsterPrefab, spawnPos);
                    if (networkObj == null)
                    {
                        Debug.LogWarning("Failed to spawn monster.");
                        continue;
                    }

                    MonsterNetworkBehaviour mnb = networkObj.GetComponent<MonsterNetworkBehaviour>();
                    if (mnb != null)
                    {
                        var agent = mnb.GetComponent<NavMeshAgent>();
                        if (agent != null) agent.enabled = true;

                        mnb.target = waveTarget;

                        if(mnb is Monster_PJ_HI pj)
                        {
                            pj.FSM.ChangePhase<PJ_HI_Phase_Chase>();
                        }
                    }
                }

                remainSpawnNumber -= num;
                iteral--;
            }
        }
    }
    private void OnGUI()
    {
        if (Runner != null)
        {
            if(Runner.IsServer)
            {
                if (GUI.Button(new Rect(10, Screen.height - 50, 150, 40), "스폰 몬스터"))
                {
                    if (points.Count > 0)
                    {
                        SpawnMonsterOnWave(transform);
                    }
                }
            }

            int allHitboxes = FindObjectsOfType<Hitbox>().Length;
            int activeHitboxes = FindObjectsOfType<Hitbox>().Count(hb => hb.enabled && hb.gameObject.activeInHierarchy);

            GUI.Label(new Rect(10, Screen.height - 100, 150, 40), $"All: {allHitboxes}");
            GUI.Label(new Rect(10, Screen.height - 150, 150, 40), $"Active: {activeHitboxes}");
        }
    }
}