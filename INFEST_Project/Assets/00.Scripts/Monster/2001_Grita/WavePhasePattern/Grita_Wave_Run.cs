using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave Phase�� ù��° ����
// ScreamWave�� �켱���� > AttackWave�� �켱����
public class Grita_Wave_Run : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;

    }
    public override void Execute()
    {
        base.Execute();

        monster.AIPathing.SetDestination(monster.target.position);

        // ������ ���˾�ä�°� ���߿� �����غ���
        // �Ÿ� 15 �̳��� �� Scream
        if (monster.AIPathing.remainingDistance <= 15f)
        {
            // Scream�� ���� �켱����, Attack�� ���� �켱����
            phase.ChangeState<Grita_ScreamWave>();
            // ��Ÿ�� ���̰ų�, �ƴϸ� 2�� �����ߴٸ� Attack
            if (!monster.CanScream() || monster.screamCount >= Monster_Grita.screamMaxCount)
                phase.ChangeState<Grita_Wave_Attack>();
        }
        else if (!monster.IsLookPlayer() && monster.AIPathing.remainingDistance > 15f)
        {
            monster.TryRemoveTarget(monster.target);
            monster.FSM.ChangePhase<Grita_Phase_Wander>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
