using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class Grenade : Consume
{
    [Networked] TickTimer _throwTimer { get; set; }
    [Networked] TickTimer _lifeTimer { get; set; }

    public NetworkPrefabRef projectilePrefab;
    public Transform throwPoint;
    float _velocity = 20f;
    Vector3 _gravity = new Vector3(0, -9.81f, 0);
    float _time;
    float _radius = 1f;
    public void Throw()
    {
        if (!_throwTimer.ExpiredOrNotRunning(Runner)) return;

        RunAnimation();
        GrenadeCreate();
        // 수류탄 나가야함;

        _throwTimer = TickTimer.CreateFromTicks(Runner, 5); // 애니메이션 시간이랑 동일하게

        // 총 안보이게 하자 => 랜더(?) 없애주자
    }

    private void GrenadeCreate()
    {
        Runner.Spawn(projectilePrefab, throwPoint.position,quaternion.identity); // 생성
        _lifeTimer = TickTimer.CreateFromTicks(Runner, 5);
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority) return;
        _time += Time.deltaTime;
        Vector3 displacement = throwPoint.position * _time + 0.5f * _gravity * _time * _time;

        transform.position += throwPoint.position * Runner.DeltaTime;

        if (_lifeTimer.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
    }

}
