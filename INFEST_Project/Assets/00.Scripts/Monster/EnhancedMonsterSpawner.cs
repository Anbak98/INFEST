using Fusion;
using INFEST.Game;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

public class EnhancedMonsterSpawner : NetworkBehaviour
{
    [SerializeField] private LayerMask spawnPointLayerMask;
    [SerializeField] private MonsterScriptableObject MonsterMap;

    public int WaveNum = 0;
    public int SpawnedNum = 0;
    public int SpawnedLimit = 51;
    public int SpawnWaitingNum = 0;

    private TickTimer waveSpawnDelayTimer;
    private List<UnityEngine.Collider> waveSpawnPoints = new();
    [SerializeField] private Queue<int> waveMonsterSpawnQueue = new();
    private Transform waveCaller;
    private Dictionary<int, SpawnTable> monsterSpawnTables = new();


    private TickTimer fieldSpawnDelayTimer;

    public override void Spawned()
    {
        base.Spawned();
        MonsterMap.Init();
        monsterSpawnTables = DataManager.Instance.GetDictionary<SpawnTable>();
    }

    public override void FixedUpdateNetwork()
    {
        if (SpawnWaitingNum <= 0)
        {
            NetworkGameManager.Instance.gameState = GameState.None;
        }

        if (waveSpawnDelayTimer.ExpiredOrNotRunning(Runner))
        {
            if (NetworkGameManager.Instance.gameState == GameState.Wave && SpawnedNum < SpawnedLimit && waveMonsterSpawnQueue.Count > 0 && waveSpawnPoints.Count > 0)
            {
                int rand = Random.Range(0, waveSpawnPoints.Count);
                NetworkObject monster = Runner.Spawn(MonsterMap.GetByKey(waveMonsterSpawnQueue.Dequeue()), waveSpawnPoints[rand].transform.position);
                MonsterNetworkBehaviour mnb = monster.GetComponent<MonsterNetworkBehaviour>();

                mnb.TryAddTarget(waveCaller);
                mnb.SetTarget(waveCaller);
                waveSpawnDelayTimer = TickTimer.CreateFromSeconds(Runner, 0.2f);

                SpawnWaitingNum--;
            }
        }

        if (fieldSpawnDelayTimer.ExpiredOrNotRunning(Runner))
        {
            if (NetworkGameManager.Instance.gameState == GameState.None)
            {

            }
        }
    }

    public void CallWave(Transform from)
    {
        int distance = 15;
        int iteral = 5;

        do
        {
            waveSpawnPoints = Physics.OverlapSphere(from.position, distance, spawnPointLayerMask).ToList();
            distance *= 2;
            iteral--;
        } while (waveSpawnPoints.Count == 0 && iteral > 0);

        if (waveSpawnPoints.Count > 0)
        {
            waveMonsterSpawnQueue = new();

            int totalSpawnNum;

            if (NetworkGameManager.Instance.gameState == GameState.Wave)
            {
                totalSpawnNum = Random.Range(7, 11);

                for (int i = 0; i < totalSpawnNum; ++i)
                {
                    int proba = 0;
                    int rand = Random.Range(0, 100);

                    foreach(var mst in monsterSpawnTables)
                    {
                        proba += (int)(mst.Value.StartByScream + mst.Value.ScreamPer5Min);

                        if(proba >= rand)
                        {
                            waveMonsterSpawnQueue.Enqueue(mst.Key);
                        }
                    }
                }
            }
            else
            {
                if (Runner.SimulationTime < 300)
                    totalSpawnNum = 30;
                else if (Runner.SimulationTime < 600)
                    totalSpawnNum = 35;
                else if (Runner.SimulationTime < 900)
                    totalSpawnNum = 40;
                else if (Runner.SimulationTime < 1200)
                    totalSpawnNum = 45;
                else
                    totalSpawnNum = 50;

                while (totalSpawnNum > 0)
                {
                    foreach (var mst in monsterSpawnTables)
                    {
                        if (totalSpawnNum <= 0)
                            break;
                        int spawnNum = mst.Value.StartByWave + mst.Value.WavePer5Min * (int)(Runner.SimulationTime / 300);

                        for (int i = 0; i < spawnNum; i++)
                        {
                            totalSpawnNum--;
                            SpawnWaitingNum++;
                            waveMonsterSpawnQueue.Enqueue(mst.Key);
                        }
                    }
                }
            }

            NetworkGameManager.Instance.gameState = GameState.Wave;

            waveCaller = from;
            WaveNum++;
        }
    }

    public int justSpawnMonsterKey = -1;

    public void SpawnOnPos(Transform pos, int monsterKey = 1001)
    {
        NetworkGameManager.Instance.gameState = GameState.None;
        NetworkObject monster = Runner.Spawn(MonsterMap.GetByKey(monsterKey), pos.transform.position);
    }

    public void JustFieldSpawn(Transform from, int? monsterKey = -1)
    {
        waveCaller = from;

        if (monsterKey == -1)
            monsterKey = justSpawnMonsterKey;

        int distance = 15;
        int iteral = 5;

        List<UnityEngine.Collider> justSpawnPoint;

        do
        {
            justSpawnPoint = Physics.OverlapSphere(from.position, distance, spawnPointLayerMask).ToList();
            distance *= 2;
            iteral--;
        } while (justSpawnPoint.Count == 0 && iteral > 0);

        NetworkGameManager.Instance.gameState = GameState.None;


        NetworkObject monster = Runner.Spawn(MonsterMap.GetByKey(monsterKey.Value), justSpawnPoint[Random.Range(0, justSpawnPoint.Count)].transform.position);
        MonsterNetworkBehaviour mnb = monster.GetComponent<MonsterNetworkBehaviour>();

        mnb.TryAddTarget(waveCaller);
        mnb.SetTarget(waveCaller);
    }

    public void JustWaveSpawn(Transform from, int? monsterKey = -1)
    {
        waveCaller = from;

        int distance = 15;
        int iteral = 5;

        if (monsterKey == -1)
            monsterKey = justSpawnMonsterKey;

        List<UnityEngine.Collider> justSpawnPoint;

        do
        {
            justSpawnPoint = Physics.OverlapSphere(from.position, distance, spawnPointLayerMask).ToList();
            distance *= 2;
            iteral--;
        } while (justSpawnPoint.Count == 0 && iteral > 0);

        NetworkGameManager.Instance.gameState = GameState.Wave;


        NetworkObject monster = Runner.Spawn(MonsterMap.GetByKey(monsterKey.Value), justSpawnPoint[Random.Range(0, justSpawnPoint.Count)].transform.position);
        MonsterNetworkBehaviour mnb = monster.GetComponent<MonsterNetworkBehaviour>();

        mnb.TryAddTarget(waveCaller);
        mnb.SetTarget(waveCaller);
    }
}