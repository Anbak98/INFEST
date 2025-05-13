public class Monster_PJ_HI : BaseMonster<Monster_PJ_HI>
{    
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
