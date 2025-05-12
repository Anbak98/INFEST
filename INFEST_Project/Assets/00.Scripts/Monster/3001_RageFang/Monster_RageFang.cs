using Fusion;

public class Monster_RageFang : BaseMonster<Monster_RageFang>
{
    [Networked, OnChangedRender(nameof(OnIsStretchChanged))]
    public NetworkBool IsStretch { get; set; } = false;

    public void OnIsStretchChanged()
    {
        animator.SetBool("IsStretch", IsStretch);
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", MovementSpeed);
        //if(IsAttack)
        //{
        //    animator.SetTrigger("IsAttack");
        //}
        //if (IsDead)
        //{
        //    FSM.ChangePhase<PJ_HI_DeadPhase>();
        //    animator.SetBool("IsDead", IsDead);
        //}

        //AIPathing.speed = MovementSpeed;
    }
}
