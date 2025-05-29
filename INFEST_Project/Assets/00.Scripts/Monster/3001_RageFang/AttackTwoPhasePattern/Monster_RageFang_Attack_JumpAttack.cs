using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_JumpAttack : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Attack>
{
    [SerializeField] ParticleSystem _particleSystem;
    public LayerMask layerMask;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 8;
        monster.IsJumpAttack = true;
        phase.skillCoolDown[6] = TickTimer.CreateFromSeconds(Runner, monster.skills[6].CoolDown);
        monster.IsReadyForChangingState = false;
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsJumpAttack = false;
    }

    public override void Attack()
    {
        base.Attack();

        UnityEngine.Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15, layerMask);

        foreach (UnityEngine.Collider collider in hitColliders)
        {
            monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[6].DamageCoefficient));
        }
    }

    public override void Effect()
    {
        base.Effect();
        monster.CurMovementSpeed = 0;
        RPC_PlayEffect();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_PlayEffect()
    {
        _particleSystem.Play();
    }
}
