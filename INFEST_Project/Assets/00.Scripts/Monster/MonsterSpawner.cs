using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : NetworkBehaviour
{
    private int SpawnMonsterNumberOnEachWave = 50;

    public List<MonsterSpawnPoint> pointRefs;

    public void SpawnMonsterOnWave()
    {
        foreach (var point in pointRefs)
        {
            Debug.Log(point.transform.position);
        }
        if (Runner.IsServer)
        {
            int remainSpawnNumber = SpawnMonsterNumberOnEachWave;
            int iteral = 20;
            while(remainSpawnNumber > 0 && iteral > 0)
            {
                int point = Random.Range(0, pointRefs.Count);
                int num = Random.Range(0, 8);
                num = num > remainSpawnNumber ? remainSpawnNumber : num;
                pointRefs[point].Spawn(num);
                remainSpawnNumber -= num;
                iteral--; 
            }
        }
    }        
}