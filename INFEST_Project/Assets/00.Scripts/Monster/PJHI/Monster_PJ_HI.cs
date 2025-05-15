using UnityEngine;

public class Monster_PJ_HI : BaseMonster<Monster_PJ_HI>
{
    public override void OnWave(Transform target)
    {
        base.OnWave(target);
        TryAddTarget(target);
        SetTarget(target);
        FSM.ChangePhase<PJ_HI_Phase_Chase>();
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
        if(IsAttack)
        {
            animator.SetTrigger("IsAttack");
        }
        if (IsDead)
        {
            animator.SetBool("IsDead", IsDead);
        }

        AIPathing.speed = CurMovementSpeed;
    }
}
