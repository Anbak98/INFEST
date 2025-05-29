using Fusion;
using UnityEngine;

public class BossSpawner : NetworkBehaviour
{
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private NetworkPrefabRef bossPrefab;

    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasStateAuthority)
        {
            StartCoroutine(SpawnBossAfterDelay());
        }
    }

    private System.Collections.IEnumerator SpawnBossAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 3ºÐ ´ë±â

        Runner.Spawn(bossPrefab, new Vector3(-45, 0, 45));
    }
}
