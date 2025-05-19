using System;
using Fusion;
using UnityEngine;

public class Bowmeter_Pattern2 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    public LayerMask collisionLayers;
    public VomitRazer vomitRazer;

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsBwack = true;
        monster.CurMovementSpeed = 0f;

        phase.skillCoolDown[2] = TickTimer.CreateFromSeconds(Runner, 7f);
    }

    public override void Execute()
    {
        base.Execute();

        Vector3 dir = (monster.target.position - monster.transform.position).normalized;
        dir.y = 0f;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, targetRot, Time.deltaTime * 5f);
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsBwack = false;
    }

    public override void Effect()
    {
        base.Effect();

        Vector3 vomitPos = phase.vomitPosition.position;
        Vector3 targetPos = monster.target.position;

        Vector3 direction = (targetPos - vomitPos).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        var spawnedVomit = Runner.Spawn(vomitRazer, vomitPos, lookRotation);        

        if (spawnedVomit != null)
        {
            spawnedVomit.GetComponent<VomitRazer>().ownerPattern2 = this;
        }
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[2].DamageCoefficient));
    }
}
