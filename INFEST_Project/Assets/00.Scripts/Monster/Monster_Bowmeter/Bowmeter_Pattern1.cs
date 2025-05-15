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
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsPunch = false;
    }

    public override void Effect()
    {
        base.Effect();

        var spawnedVomit = Runner.Spawn(vomit, phase.vomitPosition.position, phase.vomitPosition.rotation);
                
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
