using UnityEngine;

public class Monster_Stacker : BaseMonster<Monster_Stacker>
{
    public override void OnWave(Transform target)
    {
        base.OnWave(target);
        TryAddTarget(target);
        SetTarget(target);
        FSM.ChangePhase<Stacker_Phase_Chase>();
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
        if (IsAttack)
        {
            animator.SetTrigger("IsAttack");
        }
        if (IsDead)
        {
            //FSM.ChangePhase<PJ_HI_II_DeadPhase>();
            animator.SetBool("IsDead", IsDead);
        }

        AIPathing.speed = CurMovementSpeed;
    }
}
