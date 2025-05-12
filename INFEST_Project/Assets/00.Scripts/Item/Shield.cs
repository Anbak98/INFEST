using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class Shield : Consume
{
    private TickTimer _shieldTimer;
    public NetworkPrefabRef mountingPrefab;
    public Transform mountingPoint;
    

    public override void Mounting()
    {
        Debug.Log("Shield »£√‚");

        if (!_shieldTimer.ExpiredOrNotRunning(Runner)) return;
        if (HasInputAuthority)
            Player.local.inventory.RemoveConsumeItem(2);

        ShieldCreate();
    }

    private void ShieldCreate()
    {
        if (!_shieldTimer.ExpiredOrNotRunning(Runner)) return;

        Vector3 direction = _player.transform.forward;

        if (Object.HasStateAuthority)
        {
            Runner.Spawn(
            mountingPrefab,
            mountingPoint.position,
            quaternion.identity,
            Object.InputAuthority
            );
        }
        _shieldTimer = TickTimer.CreateFromSeconds(Runner, 0.5f);
    }

}
