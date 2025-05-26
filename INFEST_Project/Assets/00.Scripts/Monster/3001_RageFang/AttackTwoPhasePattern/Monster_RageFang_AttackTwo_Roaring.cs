using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_AttackTwo_Roaring : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_AttackTwo>
{
    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySfx(Sfxs.RageFang_Roaring);
        int curDef = monster.CurDef;
        
        monster.CurDef = 9999;
        monster.CurMovementSpeed = 0;
        monster.IsReadyForChangingState = false;

        monster.IsRoaring = true;

        monster.CurDamage += monster.CurDamage / 20;
        curDef += curDef / 20;
        monster.CurDef = curDef;
    }

    public override void Exit()
    {
        base.Exit();

        monster.IsPhaseAttackTwo = true;
    }
}
