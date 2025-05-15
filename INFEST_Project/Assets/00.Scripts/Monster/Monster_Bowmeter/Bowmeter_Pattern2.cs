using Fusion;
using UnityEngine;

public class Bowmeter_Pattern2 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    public LayerMask collisionLayers;
    public Vomit vomit;

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
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsBwack = false;
    }

    public override void Effect()
    {
        base.Effect();
        var spawnedVomit = Runner.Spawn(vomit, phase.vomitPosition.position, phase.vomitPosition.rotation);

        if (spawnedVomit != null)
        {
            spawnedVomit.GetComponent<Vomit>().ownerPattern2 = this;
        }
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[2].DamageCoefficient));
    }
}
