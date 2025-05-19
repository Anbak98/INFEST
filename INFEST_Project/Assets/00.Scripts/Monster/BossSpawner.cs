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
        yield return new WaitForSeconds(10f); // 3�� ���

        Runner.Spawn(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation, Object.InputAuthority);
    }
}
