using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_DeadCop : BaseMonster<Monster_DeadCop>
{
    public TickTimer animTickTimer;   // 애니메이션의 재생시간동안 다른 동작으로 못바꾼다(특히 공격)

    [Networked, OnChangedRender(nameof(OnIsHeadButt))]
    public NetworkBool IsHeadButt { get; set; } = false;

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);

        if (IsAttack)
        {
            animator.SetTrigger("IsAttack");
        }
        if (IsDead)
        {
            animator.SetBool("IsDead", IsDead);
        }
        AIPathing.speed = CurMovementSpeed;
    }
    private void OnIsHeadButt() => animator.SetBool("IsHeadButt", IsHeadButt);

    public float GetCurrentAnimLength()
    {
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0)
        {
            return clipInfos[0].clip.length;
        }
        return 0f;
    }
}
