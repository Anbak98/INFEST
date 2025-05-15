using Fusion;
using UnityEngine;

public class Bowmeter_Pattern3 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    public NetworkPrefabRef vomitAndArea;
    public LayerMask collisionLayers;

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsShoot = true;
        monster.CurMovementSpeed = 0f;

        phase.skillCoolDown[3] = TickTimer.CreateFromSeconds(Runner, 10);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsShoot = false;
    }
    public override void Effect()
    {
        base.Effect();
        var spawnedVomit = Runner.Spawn(vomitAndArea, phase.vomitPosition.position, phase.vomitPosition.rotation);

        if (spawnedVomit != null)
        {
            spawnedVomit.GetComponent<VomitAndArea>().ownerPattern3 = this;
        }
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[3].DamageCoefficient));
    }
}
