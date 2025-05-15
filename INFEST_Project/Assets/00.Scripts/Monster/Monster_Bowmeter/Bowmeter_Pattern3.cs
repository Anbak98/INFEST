using Fusion;
using UnityEngine;

public class Bowmeter_Pattern3 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    public NetworkPrefabRef vomitAndArea;
    public LayerMask collisionLayers;

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsShoot = true;
        monster.CurMovementSpeed = 0f;

        phase.skillCoolDown[3] = TickTimer.CreateFromSeconds(Runner, 10);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsShoot = false;
    }
    public override void Effect()
    {
        base.Effect();
        Runner.Spawn(phase.vomitAndArea, phase.vomitPosition.position, phase.vomitPosition.rotation);
    }

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            if (other.TryGetComponent<PlayerMethodFromMonster>(out var bridge))
            {
                Debug.Log("¾Æ¾ß");
                Attack();
            }
            SpawnVomitArea();
            Runner.Despawn(Object);
        }
    }

    private void SpawnVomitArea()
    {
        if (vomitAndArea.IsValid)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 0f;

            Runner.Spawn(vomitAndArea, spawnPosition, Quaternion.identity);
        }
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[3].DamageCoefficient));
    }
}
