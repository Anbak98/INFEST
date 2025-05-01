public class Monster_PJ_HI_II : MonsterNetworkBehaviour

{
    public override void Render()
    {
        animator.SetFloat("MovementSpeed", MovementSpeed);
        if (IsAttack)
        {
            animator.SetTrigger("IsAttack");
        }
        if (IsDead)
        {
            //FSM.ChangePhase<PJ_HI_II_DeadPhase>();
            animator.SetBool("IsDead", IsDead);
        }

        AIPathing.speed = MovementSpeed;
    }
}
