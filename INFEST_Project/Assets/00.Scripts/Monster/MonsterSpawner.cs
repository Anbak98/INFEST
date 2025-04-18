using Fusion;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class MonsterSpawner : NetworkBehaviour
{
    private int SpawnMonsterNumberOnEachWave = 50;

    public List<MonsterSpawnPoint> monsterSpawnPoints;

    private void Awake()
    {
        //monsterSpawnPoints = Object.FindObjectsOfTypeAll();  
    }

    public void ActivatedCallBySpawnPoint(MonsterSpawnPoint spawnPoint)
    {
       
    }

    public void DeactivatedCallBySpawnPoint(MonsterSpawnPoint spawnPoint)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnMonsterOnWave()
    {

    }
}