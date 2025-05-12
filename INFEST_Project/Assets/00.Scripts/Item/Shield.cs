using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class Shield : Consume
{
    private TickTimer _shieldTimer { get; set; }
    public NetworkPrefabRef mountingPrefab;
    public Transform mountingPoint;

    public override void Mounting()
    {
        Debug.Log("Shield »£√‚");

        if (!_shieldTimer.ExpiredOrNotRunning(Runner)) return;

        Player.local.inventory.RemoveConsumeItem(2);
        ShieldCreate();
    }

    private void ShieldCreate()
    {
        if (!Object.HasStateAuthority) return;
        if (!_shieldTimer.ExpiredOrNotRunning(Runner)) return;

        Vector3 direction = Camera.main.transform.forward;

        Runner.Spawn(
            mountingPrefab,
            mountingPoint.position,
            quaternion.identity,
            Object.InputAuthority
        ).GetComponent<GrenadeProjectile>();
        _shieldTimer = TickTimer.CreateFromSeconds(Runner, 0.5f);
    }



}
