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
        Runner.Spawn(phase.vomitRazer, phase.vomitPosition.position, phase.vomitPosition.rotation);
    }

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            Debug.Log("¾Æ¾ß");
            Attack();

            Runner.Despawn(Object);
        }
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[2].DamageCoefficient));
    }
}
