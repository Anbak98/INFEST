using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_TurnAttack : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Attack>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0;
        monster.IsTurnAttack = true;
        phase.skillCoolDown[9] = TickTimer.CreateFromSeconds(Runner, monster.skills[9].CoolDown);
        monster.IsReadyForChangingState = false;
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsTurnAttack = false;
    }


    public float attackRange = 5f;         // 공격 범위
    public float attackAngle = 90f;        // 반원 (90도)
    public LayerMask targetLayer;          // 타겟 필터용 레이어
    UnityEngine.Collider[] hitColliders;

    public override void Attack()
    {
        base.Attack();

        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        // 1. 일정 반경 내의 콜라이더 검색
        hitColliders = UnityEngine.Physics.OverlapSphere(origin, attackRange, targetLayer);

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = (hitCollider.transform.position - origin).normalized;

            // 2. 각도 확인
            float angle = Vector3.Angle(forward, directionToTarget);
            if (angle <= attackAngle / 2f)
            {
                // 이 콜라이더는 전방 90도 안에 있음
                monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[9].DamageCoefficient));
            }
        }
    }
}
