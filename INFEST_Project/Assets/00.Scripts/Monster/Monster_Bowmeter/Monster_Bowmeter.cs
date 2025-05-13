using Fusion;

public class Monster_Bowmeter : BaseMonster<Monster_Bowmeter>
{
    [Networked, OnChangedRender(nameof(OnIsBwack))]
    public NetworkBool IsBwack { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsShoot))]
    public NetworkBool IsShoot { get; set; } = false;

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

    private void OnIsBwack() => animator.SetBool("IsBwack", IsBwack);
    private void OnIsShoot() => animator.SetBool("IsShoot", IsShoot);
}
