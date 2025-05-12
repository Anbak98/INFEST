using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class HealItem : Consume
{
    private Player player;
    private TickTimer _healTimer;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public override void Heal()
    {
        if (!_healTimer.ExpiredOrNotRunning(Runner)) return;
        if (!HasInputAuthority) return;

        Player.local.inventory.RemoveConsumeItem(1);

        player.statHandler.Heal(instance.data.Effect);

        _healTimer = TickTimer.CreateFromSeconds(Runner, 5f);

    }

}
