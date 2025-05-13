using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave Phase�� ù��° ����
// ScreamWave�� �켱���� > AttackWave�� �켱����
public class Grita_RunWave : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = monster.info.SpeedMoveWave;

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
                phase.ChangeState<Grita_AttackWave>();
        }
        else if (!monster.IsLookPlayer() && monster.AIPathing.remainingDistance > 15f)
        {
            monster.target = null;
            monster.FSM.ChangePhase<Grita_Phase_Wander>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.MovementSpeed = 0;
    }
}
