using Fusion;
using UnityEngine;

public class VomitRazer : NetworkBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;
    public LayerMask collisionLayers;

    private float elapsed = 0f;

    public Bowmeter_Pattern2 ownerPattern2;    

    public override void FixedUpdateNetwork()
    {
        transform.position += Runner.DeltaTime * speed * transform.forward;

        elapsed += Runner.DeltaTime;

        if (elapsed >= lifetime)
        {
            Runner.Despawn(Object);
        }
    }

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            ownerPattern2?.Attack();

            Runner.Despawn(Object);
        }
    }
}
