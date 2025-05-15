using Fusion;
using UnityEngine;

public class VomitArea : NetworkBehaviour
{
    private TickTimer despawnTimer;
    public LayerMask collisionLayers;

    public override void Spawned()
    {
        despawnTimer = TickTimer.CreateFromSeconds(Runner, 7f);
    }

    public override void FixedUpdateNetwork()
    {
        if (despawnTimer.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
    }

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            if (other.TryGetComponent<PlayerMethodFromMonster>(out var bridge))
            {
                int damage = 20; // ������ ������ ��ġ ����
                bridge.ApplyDamage(0, damage); // key�� �ʿ信 ���� ����
            }
        }
    }
}
