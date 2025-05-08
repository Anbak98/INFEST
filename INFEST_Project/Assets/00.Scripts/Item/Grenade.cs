using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class Grenade : Consume
{
    [Networked] TickTimer _throwTimer { get; set; }
    public NetworkPrefabRef projectilePrefab;
    public Transform throwPoint;


    public override void Throw()
    {
        Debug.Log("Grenade ȣ��");

        if (!_throwTimer.ExpiredOrNotRunning(Runner)) return;

        StopAnimation();
        GrenadeCreate();
        // ����ź ��������;

        _throwTimer = TickTimer.CreateFromSeconds(Runner, 0.5f); // �ִϸ��̼� �ð��̶� �����ϰ�

        // �� �Ⱥ��̰� ���� => ����(?) ��������
    }

    private void GrenadeCreate()
    {
        if (!Object.HasInputAuthority) return;

        Vector3 direction = Camera.main.transform.forward;
        Vector3 velocity = direction * 10f + Vector3.up * 5f;

        GrenadeProjectile grenade = Runner.Spawn(
            projectilePrefab,
            throwPoint.position,
            quaternion.identity,
            Object.InputAuthority
        ).GetComponent<GrenadeProjectile>();

        grenade.Init(velocity, gameObject);
    }
}
