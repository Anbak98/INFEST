using Fusion;
using UnityEngine;

public class GoreHaul_JumpAttack : MonsterStateNetworkBehaviour<Monster_GoreHaul, GoreHaul_Phase_Chase>
{
    public LayerMask layerMask;

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsJumpAttack = true;
        //monster.CurMovementSpeed = 0f;

        phase.skillCoolDown[3] = TickTimer.CreateFromSeconds(Runner, 10f);
    }

    public override void Execute()
    {
        base.Execute();

        if (monster.target == null)
            return;

        Vector3 direction = (monster.target.position - monster.transform.position).normalized;

        monster.transform.position += direction * monster.CurMovementSpeed * Runner.DeltaTime;

        //float distance = Vector3.Distance(monster.transform.position, monster.target.position);

        //if (distance > monster.skills[3].UseRange - 1f)
        //{
        //    phase.ChangeState<GoreHaul_Run>();
        //}
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsJumpAttack = false;
    }    

    public override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySfx(Sfxs.ZombieAttack);
        //monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[3].DamageCoefficient));

        UnityEngine.Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5, layerMask);

        foreach (UnityEngine.Collider collider in hitColliders)
        {
            monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[3].DamageCoefficient));
        }
    }
}
