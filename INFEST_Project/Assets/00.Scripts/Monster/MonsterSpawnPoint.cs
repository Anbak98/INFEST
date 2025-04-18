using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    public MonsterSpawner Spawner;
    public bool IsActivated = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameVisible()
    {
        IsActivated = true;
        Spawner.ActivatedCallBySpawnPoint(this);
    }

    private void OnBecameInvisible()
    {
        IsActivated = false;
        Spawner.DeactivatedCallBySpawnPoint(this);
    }
}