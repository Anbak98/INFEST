using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class Grenade : Consume
{
    private TickTimer _throwTimer;
    public NetworkPrefabRef projectilePrefab;
    public Transform throwPoint;
    private GrenadeProjectile grenade;
    
    public override void Throw()
    {
        Debug.Log("Grenade »£√‚");

        if (!_throwTimer.ExpiredOrNotRunning(Runner)) return;
    
        //_player.inventory.RemoveConsumeItem(0);

        GrenadeCreate();

    }

    private void GrenadeCreate()
    {
        Vector3 direction = _player.transform.forward;
        Vector3 velocity = direction * 10f + Vector3.up * 10f;

        if (Object.HasStateAuthority)
        {
            GrenadeProjectile _grenade = Runner.Spawn(
            projectilePrefab,
            throwPoint.position,
            quaternion.identity,
            Object.InputAuthority
            ).GetComponent<GrenadeProjectile>();

            _grenade.Init(velocity, throwPoint.position);
            RPC_Init(_grenade, velocity);
        }
        _throwTimer = TickTimer.CreateFromSeconds(Runner, 2.7f);
    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    private void RPC_Init(GrenadeProjectile _grenade, Vector3 _velocity)
    {
        grenade = _grenade;

        grenade.GetGrenade(this, _velocity);
    }
}
