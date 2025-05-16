using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Chase_DropKick : MonsterStateNetworkBehaviour<Monster_WarZ, WarZ_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("DropKick");
        monster.CurMovementSpeed = 0f;
        monster.IsDropKick = true;

        // 애니메이션이 끝나기 전에는 상태가 안바뀐다
        monster.IsReadyForChangingState = false;
    }


    public override void Exit()
    {
        base.Exit();
        monster.IsDropKick = false;
        // 애니메이션은 정지하더라도
        // Phase_Chase의 currentState가 DropKick으로 남아있으므로
        // Run으로 바꿔야한다
        // phase.ChangeState<WarZ_Chase_Run>(); // overflow 발생...
        // ChangeState에서 currentState?.Exit이 들어와서 Exit가 계속 호출되기 때문이다
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage /** monster.skills[1].DamageCoefficient*/));
    }

}
