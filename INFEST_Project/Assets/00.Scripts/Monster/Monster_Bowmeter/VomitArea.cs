using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class VomitArea : NetworkBehaviour
{
    private TickTimer despawnTimer;
    private TickTimer damageTickTimer;

    public LayerMask collisionLayers;
    public float tickInterval = 1f;
    public int tickDamage = 20;
    
    private HashSet<TargetableFromMonster> affectedPlayers = new();

    public override void Spawned()
    {
        despawnTimer = TickTimer.CreateFromSeconds(Runner, 7f);
        damageTickTimer = TickTimer.CreateFromSeconds(Runner, tickInterval);
    }

    public override void FixedUpdateNetwork()
    {
        if (despawnTimer.Expired(Runner))
        {
            Runner.Despawn(Object);
            return;
        }

        if (damageTickTimer.Expired(Runner))
        {
            foreach (var player in affectedPlayers)
            {
                player.ApplyDamage(0, tickDamage);
            }

            damageTickTimer = TickTimer.CreateFromSeconds(Runner, tickInterval);
        }
    }

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            if (other.TryGetComponent<TargetableFromMonster>(out var player))
            {
                Debug.Log("밟았다");
                affectedPlayers.Add(player);
            }
        }
    }

    public void OnTriggerExit(UnityEngine.Collider other)
    {
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            if (other.TryGetComponent<TargetableFromMonster>(out var player))
            {
                Debug.Log("나왔다");
                affectedPlayers.Remove(player);
            }
        }
    }
}
