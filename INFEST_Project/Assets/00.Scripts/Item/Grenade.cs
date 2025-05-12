using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class Grenade : Consume
{
    private TickTimer _throwTimer;
    public NetworkPrefabRef projectilePrefab;
    public Transform throwPoint;
    public Player _player;
    GrenadeProjectile grenade;
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    public override void Throw()
    {
        Debug.Log("Grenade »£√‚");

        if (!_throwTimer.ExpiredOrNotRunning(Runner)) return;
        if (HasInputAuthority)
            Player.local.inventory.RemoveConsumeItem(0);

        GrenadeCreate();

    }

    private void GrenadeCreate()
    {
        if (!_throwTimer.ExpiredOrNotRunning(Runner)) return;

        Vector3 direction = _player.transform.forward;
        Vector3 velocity = direction * 10f + Vector3.up * 5f;

        if (Object.HasStateAuthority)
        {
            grenade = Runner.Spawn(
            projectilePrefab,
            throwPoint.position,
            quaternion.identity,
            Object.InputAuthority
            ).GetComponent<GrenadeProjectile>();

            grenade.Init(velocity, throwPoint.position, this);
        }
        _throwTimer = TickTimer.CreateFromSeconds(Runner, 3f);
    }
}
