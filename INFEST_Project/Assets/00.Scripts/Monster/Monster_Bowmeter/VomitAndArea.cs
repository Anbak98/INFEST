using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class VomitAndArea : NetworkBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;
    public NetworkPrefabRef vomitAreaPrefab;
    public LayerMask collisionLayers;

    private float elapsed = 0f;

    public override void FixedUpdateNetwork()
    {
        transform.position += transform.forward * speed * Runner.DeltaTime;

        elapsed += Runner.DeltaTime;

        if (elapsed >= lifetime)
        {
            SpawnVomitArea();
            Runner.Despawn(Object);
        }
    }

    //public void OnTriggerEnter(UnityEngine.Collider other)
    //{
    //    if (((1 << other.gameObject.layer) & collisionLayers) != 0)
    //    {
    //        if (other.TryGetComponent<PlayerMethodFromMonster>(out var bridge))
    //        {
    //            int damage = 10; // 적절한 데미지 수치 설정
    //            bridge.ApplyDamage(0, damage); // key는 필요에 따라 수정
    //        }
    //        SpawnVomitArea();
    //        Runner.Despawn(Object);
    //    }
    //}

    private void SpawnVomitArea()
    {
        if (vomitAreaPrefab.IsValid)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 0f;

            Runner.Spawn(vomitAreaPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
