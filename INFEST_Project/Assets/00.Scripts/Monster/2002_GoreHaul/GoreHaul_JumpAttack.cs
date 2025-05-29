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

        Vector3 dir = (monster.target.position - monster.transform.position).normalized;
        dir.y = 0f;
        Quaternion targetRot = Quaternion.LookRotation(dir);

        monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, targetRot, Time.deltaTime * 5f);
        monster.transform.position += monster.CurMovementSpeed * Runner.DeltaTime * dir;
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
