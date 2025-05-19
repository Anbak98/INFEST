using Fusion;
using UnityEngine;

public class Bowmeter_Pattern1 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    public LayerMask collisionLayers;
    public Vomit vomit;

    public override void Enter()
    {
        base.Enter();
        monster.IsPunch = true;
        monster.CurMovementSpeed = 0f;

        phase.skillCoolDown[1] = TickTimer.CreateFromSeconds(Runner, 1f);
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
        monster.IsPunch = false;
    }

    public override void Effect()
    {
        base.Effect();
               
        Vector3 vomitPos = phase.vomitPosition.position;
        Vector3 targetPos = monster.target.position;

        Vector3 direction = (targetPos - vomitPos).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        var spawnedVomit = Runner.Spawn(vomit, vomitPos, lookRotation);       

        if (spawnedVomit != null)
        {
            spawnedVomit.GetComponent<Vomit>().ownerPattern1 = this;
        }
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[1].DamageCoefficient));
    }
}
