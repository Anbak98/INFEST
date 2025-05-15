using Fusion;
using UnityEngine;

public class VomitAndArea : NetworkBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;
    public VomitArea vomitAreaPrefab;
    public LayerMask collisionLayers;

    private float elapsed = 0f;

    public Bowmeter_Pattern3 ownerPattern3;

    public override void FixedUpdateNetwork()
    {
        transform.position += Runner.DeltaTime * speed * transform.forward;

        elapsed += Runner.DeltaTime;

        if (elapsed >= lifetime)
        {
            SpawnVomitArea();
            Runner.Despawn(Object);
        }
    }

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            ownerPattern3?.Attack();

            SpawnVomitArea();
            Runner.Despawn(Object);
        }
    }

    private void SpawnVomitArea()
    {
        Vector3 spawnPosition = transform.position;

        spawnPosition.y = 0.05f;

        Runner.Spawn(vomitAreaPrefab, spawnPosition, Quaternion.identity);
    }
}
