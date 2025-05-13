public class Monster_Bowmeter : BaseMonster<Monster_Bowmeter>
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
            animator.SetBool("IsDead", IsDead);
        }

        AIPathing.speed = MovementSpeed;
    }
}
