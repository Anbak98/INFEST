using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_AttackTwo_Jumping : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_AttackTwo>
{
    [SerializeField] ParticleSystem _particleSystem;
    public LayerMask layerMask;
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0;
        monster.IsJumping = true;
        phase.skillCoolDown[3] = TickTimer.CreateFromSeconds(Runner, monster.skills[3].CoolDown);
        monster.IsReadyForChangingState = false;
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsJumping = false;
    }
    public override void Attack()
    {
        base.Attack();

        UnityEngine.Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15, layerMask);

        foreach (UnityEngine.Collider collider in hitColliders)
        {
            monster.TryAttackTarget(collider.transform, (int)(monster.CurDamage * 1.35));
        }
    }

    public override void Effect()
    {
        base.Effect();
        RPC_PlayEffect();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_PlayEffect()
    {
        _particleSystem.Play();
    }
}
