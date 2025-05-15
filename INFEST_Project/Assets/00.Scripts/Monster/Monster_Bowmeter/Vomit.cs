using Fusion;
using UnityEngine;

public class Vomit : NetworkBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;
    public LayerMask collisionLayers;

    private float elapsed = 0f;

    public override void FixedUpdateNetwork()
    {
        transform.position += transform.forward * speed * Runner.DeltaTime;

        elapsed += Runner.DeltaTime;

        if (elapsed >= lifetime)
        {
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

    //        Runner.Despawn(Object);            
    //    }
    //}
}
