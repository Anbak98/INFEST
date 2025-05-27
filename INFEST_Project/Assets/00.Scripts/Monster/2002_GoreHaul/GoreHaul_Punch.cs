using Fusion;

public class GoreHaul_Punch : MonsterStateNetworkBehaviour<Monster_GoreHaul, GoreHaul_Phase_Chase>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();

        monster.IsPunch = true;
        monster.CurMovementSpeed = 0f;

        _tickTimer = TickTimer.CreateFromSeconds(Runner, 2f);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsPunch = false;
    }

    public override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySfx(Sfxs.ZombieAttack);
        monster.TryAttackTarget(monster.CurDamage);
    }
}
