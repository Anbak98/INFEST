using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class Grenade : Consume
{
    public NetworkPrefabRef projectilePrefab;
    public Transform throwPoint;
    private GrenadeProjectile grenade;
    
    public override void Throw()
    {
        Debug.Log("Grenade ȣ��");
        if (!timer.ExpiredOrNotRunning(Runner)) return;
    
        _player.inventory.RemoveConsumeItem(0);

        GrenadeCreate();

    }

    private void GrenadeCreate()
    {
        Vector3 camForward = throwPoint.forward.normalized;
        float camY = camForward.y;

        // ���� ���� �������� �� ���� ��ȭ
        float angleFactor = Mathf.InverseLerp(-0.2f, 0.8f, camY);
        float upwardBoost = Mathf.Lerp(0.3f, 0.9f, angleFactor); // ���� ���� 0.9���� ����

        // ���� ������ ����
        Vector3 throwDir = (camForward + Vector3.up * upwardBoost).normalized;
        Vector3 velocity = throwDir * 12f; 



        if (Object.HasStateAuthority)
        {
            GrenadeProjectile _grenade = Runner.Spawn(
            projectilePrefab,
            throwPoint.position,
            quaternion.identity,
            Object.InputAuthority
            ).GetComponent<GrenadeProjectile>();

            _grenade.Init(velocity);
            RPC_Init(_grenade);
        }
        SetCoolTime(3f);

    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    private void RPC_Init(GrenadeProjectile _grenade)
    {
        grenade = _grenade;

        grenade.GetGrenade(this);
    }
}
