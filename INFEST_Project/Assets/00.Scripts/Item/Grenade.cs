using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class Grenade : Consume
{
    private TickTimer _throwTimer { get; set; }
    public NetworkPrefabRef projectilePrefab;
    public Transform throwPoint;

    public override void Throw()
    {
        Debug.Log("Grenade 호출");

        if (!_throwTimer.ExpiredOrNotRunning(Runner)) return;

        Player.local.inventory.RemoveConsumeItem(0);
        GrenadeCreate();
        // 수류탄 나가야함;

         // 애니메이션 시간이랑 동일하게

        // 총 안보이게 하자 => 랜더(?) 없애주자
    }

    private void GrenadeCreate()
    {
        if (!Object.HasStateAuthority) return;
        if (!_throwTimer.ExpiredOrNotRunning(Runner)) return;

        Vector3 direction = Camera.main.transform.forward;
        Vector3 velocity = direction * 10f + Vector3.up * 5f;

        GrenadeProjectile grenade = Runner.Spawn(
            projectilePrefab,
            throwPoint.position,
            quaternion.identity,
            Object.InputAuthority
        ).GetComponent<GrenadeProjectile>();

        grenade.Init(velocity, this, throwPoint.position);

        _throwTimer = TickTimer.CreateFromSeconds(Runner, 3f);
    }

}
