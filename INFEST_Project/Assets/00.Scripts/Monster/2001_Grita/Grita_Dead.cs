using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Dead : MonsterStateNetworkBehaviour<Monster_Grita>
{
    public TickTimer _tickTimer;
    public NetworkObject obj;

    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 7);

    }
    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner))
        {
            if (HasStateAuthority)
            {
                Runner.Despawn(obj);
            }
        }
    }
}
