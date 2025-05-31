using Fusion;
using UnityEngine;

public class HealItem : Consume
{
    public override void Heal()
    {
        if (!timer.ExpiredOrNotRunning(Runner)) return;
        
        _player.inventory.RemoveConsumeItem(1);

        _player.statHandler.Heal(instance.data.Effect);

        SetCoolTime(5f);
    }

}
