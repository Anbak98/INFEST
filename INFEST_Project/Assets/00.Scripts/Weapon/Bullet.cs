using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [Networked] private TickTimer _life { get; set; }
    private Vector3 _charPos;
    private float _maxHitDistance;

    public void Init(Vector3 pos, float maxHitDistance)
    {
        _life = TickTimer.CreateFromSeconds(Runner, 5.0f);
        _charPos = pos;
        _maxHitDistance = maxHitDistance;
    }

    public override void FixedUpdateNetwork()
    {
        float distance = Vector3.Distance(_charPos, transform.position);
        if (distance >= _maxHitDistance)
            Runner.Despawn(Object);
        else
            transform.position += 5 * transform.forward * Runner.DeltaTime;
    }
}
