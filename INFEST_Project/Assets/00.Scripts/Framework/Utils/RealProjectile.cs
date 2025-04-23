using Fusion;
using UnityEngine;

public class RealProjectile : NetworkBehaviour
{
    [SerializeField] private float _speed = 80f;
    [SerializeField] private float _lifeTime = 2f;

    private Vector3 _direction;
    private TickTimer _lifeTimer;

    public void Init(Vector3 direction)
    {
        _direction = direction.normalized;
        _lifeTimer = TickTimer.CreateFromSeconds(Runner, _lifeTime);
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority == false) return;

        transform.position += _direction * _speed * Runner.DeltaTime;

        if (_lifeTimer.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
    }
    

    void OnCollisionEnter(Collision collision)
    {
        if (Object.HasStateAuthority == false) return;

        if (collision.rigidbody != null)
        {
            Vector3 force = _direction * 10f; // ¹Ð¾î³»´Â Èû
            collision.rigidbody.AddForce(force, ForceMode.Impulse);
        }

        Runner.Despawn(Object);
    }
}
