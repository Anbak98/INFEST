using Fusion;

public class Stacker_Attack : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Chase>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();        

        monster.IsReadyForChangingState = false;
        monster.IsPunch = true;
        monster.CurMovementSpeed = 0f;

        _tickTimer = TickTimer.CreateFromSeconds(Runner, 2);
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
