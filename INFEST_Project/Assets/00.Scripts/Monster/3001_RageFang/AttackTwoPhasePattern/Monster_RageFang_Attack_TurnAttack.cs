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


    public float attackRange = 5f;         // ���� ����
    public float attackAngle = 90f;        // �ݿ� (90��)
    public LayerMask targetLayer;          // Ÿ�� ���Ϳ� ���̾�
    UnityEngine.Collider[] hitColliders;

    public override void Attack()
    {
        base.Attack();

        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        // 1. ���� �ݰ� ���� �ݶ��̴� �˻�
        hitColliders = UnityEngine.Physics.OverlapSphere(origin, attackRange, targetLayer);

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = (hitCollider.transform.position - origin).normalized;

            // 2. ���� Ȯ��
            float angle = Vector3.Angle(forward, directionToTarget);
            if (angle <= attackAngle / 2f)
            {
                // �� �ݶ��̴��� ���� 90�� �ȿ� ����
                monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[9].DamageCoefficient));
            }
        }
    }
}
