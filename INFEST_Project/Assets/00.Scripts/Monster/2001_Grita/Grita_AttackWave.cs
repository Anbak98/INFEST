using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attack�� AttackWave����
// AttackWave�� Wave�� ���۵� �� ��������
// ��ȹ�� ���� Attack�� �ٸ� ������ �� �� �־ �и��ߴ�
public class Grita_AttackWave : MonsterStateNetworkBehaviour<Monster_Grita>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();

        if (monster.IsDead || monster.target == null)
            return;

        monster.MovementSpeed = 0f;
        monster.IsAttack = true;

        _tickTimer = TickTimer.CreateFromSeconds(Runner, 2);
    }
    public override void Execute()
    {
        base.Execute();

    }

    public override void Exit()
    {
        base.Exit();

    }

}
