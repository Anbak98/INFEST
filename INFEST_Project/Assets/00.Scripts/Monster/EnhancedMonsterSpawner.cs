using Fusion;
using INFEST.Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnhancedMonsterSpawner : NetworkBehaviour
{
    [SerializeField] private LayerMask spawnPointLayerMask;
    [SerializeField] private MonsterScriptableObject MonsterMap;

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

            foreach (var mst in monsterSpawnTables)
            {
                int spawnNum = mst.Value.StartByWave + mst.Value.WavePer5Min * (int)(Runner.SimulationTime / 300);

                if(NetworkGameManager.Instance.gameState != GameState.Wave)
                {
                    spawnNum *= 5;
                }

                for (int i = 0; i < spawnNum; i++)
                {
                    SpawnWaitingNum++;
                    waveMonsterSpawnQueue.Enqueue(mst.Key);
                }
            }

            NetworkGameManager.Instance.gameState = GameState.Wave;

            waveCaller = from;
        }
    }

    public int justSpawnMonsterKey = -1;

    public void JustFieldSpawn(Transform from)
    {
        waveCaller = from;

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


        NetworkObject monster = Runner.Spawn(MonsterMap.GetByKey(justSpawnMonsterKey), justSpawnPoint[Random.Range(0, justSpawnPoint.Count)].transform.position);
        MonsterNetworkBehaviour mnb = monster.GetComponent<MonsterNetworkBehaviour>();

        mnb.TryAddTarget(waveCaller);
        mnb.SetTarget(waveCaller);
    }

    public void JustWaveSpawn(Transform from)
    {
        waveCaller = from;

        int distance = 15;
        int iteral = 5;

        List<UnityEngine.Collider> justSpawnPoint;

        do
        {
            justSpawnPoint = Physics.OverlapSphere(from.position, distance, spawnPointLayerMask).ToList();
            distance *= 2;
            iteral--;
        } while (justSpawnPoint.Count == 0 && iteral > 0);

        NetworkGameManager.Instance.gameState = GameState.Wave;


        NetworkObject monster = Runner.Spawn(MonsterMap.GetByKey(justSpawnMonsterKey), justSpawnPoint[Random.Range(0, justSpawnPoint.Count)].transform.position);
        MonsterNetworkBehaviour mnb = monster.GetComponent<MonsterNetworkBehaviour>();

        mnb.TryAddTarget(waveCaller);
        mnb.SetTarget(waveCaller);
    }
}