using Fusion;

public class HealItem : Consume
{
    private TickTimer _healTimer;


    public override void Heal()
    {
        if (!_healTimer.ExpiredOrNotRunning(Runner)) return;
        
        _player.inventory.RemoveConsumeItem(1);

        _player.statHandler.Heal(instance.data.Effect);

        _healTimer = TickTimer.CreateFromSeconds(Runner, 5f);

    }

}
