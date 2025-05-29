using INFEST.Game;
using UnityEngine;

public class Monster_RageFang_Retreat_Roaring : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Retreat>
{
    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySfx(Sfxs.RageFang_Roaring);

        monster.Buff(0, 9999);
        monster.Buff(monster.BaseDamage / 20, monster.BaseDef / 20);
        monster.CurMovementSpeed = 0;
        monster.IsReadyForChangingState = false;
        monster.IsRoaring = true;
    }

    public override void Exit()
    {
        base.Exit();

        monster.IsRoaring = false;
        monster.Buff(0, -9999);
    }

    public override void Attack()
    {
        base.Attack();
        if (monster.target != null)
        {
            NetworkGameManager.Instance.monsterSpawner.CallWave(monster.target);

        }
    }
}
