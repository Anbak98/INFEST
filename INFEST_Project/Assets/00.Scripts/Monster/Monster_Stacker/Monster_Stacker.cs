public class Monster_Stacker : BaseMonster<Monster_Stacker>

{
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
