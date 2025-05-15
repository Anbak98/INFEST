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
    TickTimer _animTickTimer;
    public override void Enter()
    {
        base.Enter();
        // Scream
        monster.IsScream = true;
        monster.IsCooltimeCharged = false;  // ��� ������
        monster.ScreamCount++;

        Debug.Log("Scream Wave Enter");

        monster.Rpc_Scream();
        float animLength = monster.GetCurrentAnimLength();

        _animTickTimer = TickTimer.CreateFromSeconds(Runner, animLength);   // �ִϸ��̼� ��� �ð� Ÿ�̸�

        // Spawn
        monster.StartCoroutine(monster.SpawnAfterAnim(animLength));
    }
    public override void Execute()
    {
        // �ڷ�ƾ ���� ���� ����Ǵ��� Ȯ��
        Debug.Log("Execute ����");
        base.Execute();

        // Enter���� ���������� RunWave�� ������ȯ
        if (_animTickTimer.Expired(Runner))     // ��� ������ true
        {
            phase.ChangeState<Grita_Wave_Run>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsScream = false;
        Debug.Log("Scream Wave Exit");
    }
}

// Wave phase
// MonsterSpawner�� �˾Ƽ� ���ִϱ� ���� �����Ǵ°� RPC�� �ʿ����� �ʴ�
// SpawnMonsterOnWave���� ���������� Runner.Spawn���� ����
// host���� 1���� ȣ���ϸ� �ȴ�
// ���͸� 7~10 �߰� ����
// ���ͺ��� ���� Ȯ���� �ٸ��ϱ� ��ȹ���� ����
