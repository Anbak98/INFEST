using Fusion;

public class GoreHaul_LowAttack : MonsterStateNetworkBehaviour<Monster_GoreHaul, GoreHaul_Phase_Chase>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();

        monster.IsLowAttack = true;
        monster.CurMovementSpeed = 0f;

        _tickTimer = TickTimer.CreateFromSeconds(Runner, 5f);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsLowAttack = false;
    }

    public override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySfx(Sfxs.ZombieAttack);
        monster.TryAttackTarget(monster.CurDamage);
    }
}
