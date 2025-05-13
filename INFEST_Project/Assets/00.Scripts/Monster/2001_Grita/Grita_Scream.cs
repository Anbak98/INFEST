using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave�� �ƴϴ�
// Scream���¿��� �� ��
// �Ҹ� ������
public class Grita_Scream : MonsterStateNetworkBehaviour<Monster_Grita>
{
    public override void Enter()
    {
        base.Enter();
        // Scream
        // ��Ÿ��, Ƚ�� ����
        if (monster.CanScream() && monster.screamCount < Monster_Grita.screamMaxCount)
        {
            monster.Rpc_Scream();
            float animLength = monster.GetCurrentAnimLength();
            // Spawn
            monster.StartCoroutine(monster.SpawnAfterAnim(animLength));
        }
    }
    public override void Execute()
    {
        base.Execute();
        // �߰� phase�� ��ȯ
        monster.FSM.ChangePhase<Grita_Phase_Chase>(); // 0�� ���� Run�� ����
    }

    public override void Exit()
    {
        base.Exit();
    }
}

