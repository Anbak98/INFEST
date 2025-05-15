using Fusion;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterSpawner : NetworkBehaviour
{
    // TickTimer timer; // 나중에 타이머로 경보기하면 될듯
    public int SpawnMonsterNumberOnEachWave = 5;
    [SerializeField] private List<Transform> points;
    [SerializeField] private MonsterScriptableObject MonsterMap;
    [SerializeField] private int MonsterKey = 1001;

    private TickTimer tickTimer;
    private int spawnNum = 0;
    private int monsterKey;
    private Vector3 spawnPosition;


    public override void Spawned()
    {
        MonsterMap.Init();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (tickTimer.ExpiredOrNotRunning(Runner))
        {
            if (spawnNum > 0)
            {
                if (Runner.IsServer)
                {
                    Runner.Spawn(MonsterMap.GetByKey(monsterKey), spawnPosition);
                    tickTimer = TickTimer.CreateFromSeconds(Runner, 0.1f);
                    --spawnNum;
                }
            }
        }
    }    
    public void AllocateSpawnCommand(int monsterKey, int monsterNum,Vector3 position)
    {
        if (spawnNum > 0)
        {
            Debug.Log("[Monster Spawner] Already Spawning Precessing");
        }
        else
        {
            this.spawnNum = monsterNum;
            this.monsterKey = monsterKey;
            this.spawnPosition = position;
        }
    }

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

                    Debug.Log(MonsterMap.GetByKey(MonsterKey));  
                    NetworkObject networkObj = Runner.Spawn(MonsterMap.GetByKey(MonsterKey), spawnPos);
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

                        mnb.SetTarget(waveTarget);

                        if(mnb is Monster_PJ_HI pj)
                        {
                            pj.FSM.ChangePhase<PJ_HI_Phase_Chase>();
                        }

                        if (mnb is Monster_Stacker st)
                        {
                            st.FSM.ChangePhase<Stacker_Phase_Chase>();
                        }

                        if (mnb is Monster_Grita grita)
                        {
                            grita.spawner = FindObjectOfType<MonsterSpawner>(); 
                            grita.FSM.ChangePhase<Grita_Phase_Wander>();
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