public class Monster_PJ_HI : MonsterNetworkBehaviour
{    
    public override void Render()
    {
        animator.SetFloat("MovementSpeed", MovementSpeed);
        if(IsAttack)
        {
            animator.SetTrigger("IsAttack");
        }
        if (IsDead)
        {
            FSM.ChangePhase<PJ_HI_DeadPhase>();
            animator.SetBool("IsDead", IsDead);
        }

        AIPathing.speed = MovementSpeed;
    }
}
