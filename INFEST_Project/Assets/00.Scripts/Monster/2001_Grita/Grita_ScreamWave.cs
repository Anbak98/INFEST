using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScreamWave���¿��� �� ��
// �Ҹ� ������
// ���� �����ϱ�

/// <summary>
/// ���̺긦 �߸� �����ؼ� �ٲٴ� ���̴�
/// �̰� �ӽ÷� �����
///
/// </summary>
public class Grita_ScreamWave : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();
        // Scream
        monster.IsScream = true;

        monster.Rpc_Scream();
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsScream = false;
    }
}

// Wave phase
// MonsterSpawner�� �˾Ƽ� ���ִϱ� ���� �����Ǵ°� RPC�� �ʿ����� �ʴ�
// SpawnMonsterOnWave���� ���������� Runner.Spawn���� ����
// host���� 1���� ȣ���ϸ� �ȴ�
// ���͸� 7~10 �߰� ����
// ���ͺ��� ���� Ȯ���� �ٸ��ϱ� ��ȹ���� ����
