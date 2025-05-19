using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_AttackTwo_Roaring : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_AttackTwo>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0;
        monster.IsReadyForChangingState = false;

        monster.IsRoaring = true;

        monster.CurDamage += monster.CurDamage / 20;
        monster.CurDef += monster.CurDef / 20;
    }
}
