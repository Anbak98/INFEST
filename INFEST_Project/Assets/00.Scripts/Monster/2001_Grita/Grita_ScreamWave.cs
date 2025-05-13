using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScreamWave���¿��� �� ��
// �Ҹ� ������
// ���� �����ϱ�

public class Grita_ScreamWave : MonsterStateNetworkBehaviour<Monster_Grita>
{

    public override void Enter()
    {
        base.Enter();
        // Scream
        if (monster.CanScream() && monster.screamCount < Monster_Grita.screamMaxCount)
        {
            monster.Rpc_Scream();   // ���⼭ ��� �ع�����.. ���¸ӽſ��� �� ���� ����
            float animLength = monster.GetCurrentAnimLength();
            // Spawn
            monster.StartCoroutine(monster.SpawnAfterAnim(animLength));
        }
    }
    public override void Execute()
    {
        // �ڷ�ƾ ���� ���� ����Ǵ��� Ȯ��
        Debug.Log("Execute ����");
        base.Execute();

        // Enter���� ���������� RunWave�� ������ȯ
        phase.ChangeState<Grita_RunWave>();

    }

    public override void Exit()
    {
        base.Exit();

    }

}

// �߰� ����
// MonsterSpawner�� �˾Ƽ� ���ִϱ� ���� �����Ǵ°� RPC�� �ʿ����� �ʴ�
// SpawnMonsterOnWave���� ���������� Runner.Spawn���� ����
// host���� 1���� ȣ���ϸ� �ȴ�

// ���͸� 7~10 �߰� ����
// ���ͺ��� ���� Ȯ���� �ٸ��ϱ� ��ȹ���� ����
