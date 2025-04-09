using Fusion;
using UnityEngine;

public class RealProjectile : NetworkBehaviour
{
    [SerializeField] private float _speed = 80f;
    [SerializeField] private float _lifeTime = 2f;

    private Vector3 _direction;
    private float _spawnTime;

    public void Init(Vector3 direction)
    {
        _direction = direction.normalized;
        _spawnTime = Time.time;
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority == false) return;

        transform.position += _direction * _speed * Runner.DeltaTime;

        if (Time.time - _spawnTime > _lifeTime)
        {
            Runner.Despawn(Object);
        }
    }
}
