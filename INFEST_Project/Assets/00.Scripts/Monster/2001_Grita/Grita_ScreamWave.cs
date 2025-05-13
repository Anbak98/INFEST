using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScreamWave���¿��� �� ��
// �Ҹ� ������
// ���� �����ϱ�

public class Grita_ScreamWave : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wave>
{
    public TickTimer _tickTimer;

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

        _tickTimer = TickTimer.CreateFromSeconds(Runner, animLength);   // �ش� �ð��� ���� ���� ���� ����

        // Spawn
        //monster.StartCoroutine(monster.SpawnAfterAnim(animLength));
    }
    public override void Execute()
    {
        // �ڷ�ƾ ���� ���� ����Ǵ��� Ȯ��
        Debug.Log("Execute ����");
        base.Execute();

        // Enter���� ���������� RunWave�� ������ȯ
        if (_tickTimer.Expired(Runner))     // _tickTimer�� �ش� �ð���ŭ ������ true�� �ȴ�
        {
            phase.ChangeState<Grita_RunWave>();
        }

    }

    public override void Exit()
    {
        base.Exit();
        monster.IsScream = false;
        Debug.Log("Scream Wave Exit");
    }
}

// �߰� ����
// MonsterSpawner�� �˾Ƽ� ���ִϱ� ���� �����Ǵ°� RPC�� �ʿ����� �ʴ�
// SpawnMonsterOnWave���� ���������� Runner.Spawn���� ����
// host���� 1���� ȣ���ϸ� �ȴ�

// ���͸� 7~10 �߰� ����
// ���ͺ��� ���� Ȯ���� �ٸ��ϱ� ��ȹ���� ����
