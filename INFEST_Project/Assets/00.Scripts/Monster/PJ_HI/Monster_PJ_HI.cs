public class Monster_PJ_HI : MonsterNetworkBehaviour
{
    public override void Render()
    {
        animator.SetFloat("MovementSpeed", MovementSpeed);
        animator.SetBool("IsAttack", IsAttack);
        if (IsDead)
        {
            FSM.ChangePhase<PJ_HI_DeadPhase>();
            animator.SetBool("IsDead", IsDead);
        }

        AIPathing.speed = MovementSpeed;
    }
}
