using Fusion;

public class GoreHaul_JumpAttack : MonsterStateNetworkBehaviour<Monster_GoreHaul, GoreHaul_Phase_Chase>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();

        monster.IsJumpAttack = true;
        monster.CurMovementSpeed = 0f;

        _tickTimer = TickTimer.CreateFromSeconds(Runner, 20f);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsJumpAttack = false;
    }

    public override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySfx(Sfxs.ZombieAttack);
        monster.TryAttackTarget(monster.CurDamage);
    }
}
