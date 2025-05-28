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

        phase.skillCoolDown[3] = TickTimer.CreateFromSeconds(Runner, 10f);
    }

    public override void Execute()
    {
        base.Execute();

        if (monster.target == null)
            return;

        Vector3 direction = (monster.target.position - monster.transform.position).normalized;

        monster.transform.position += monster.CurMovementSpeed * Runner.DeltaTime * direction;
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

        UnityEngine.Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5, layerMask);

        foreach (UnityEngine.Collider collider in hitColliders)
        {
            monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[3].DamageCoefficient));
        }
    }
}
