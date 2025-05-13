using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave�� �ƴϴ�
// Scream���¿��� �� ��
// �Ҹ� ������
public class Grita_Scream : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wander>
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        // Scream
        monster.IsScream = true;
        monster.ScreamCount++;
        Debug.Log("Scream Enter");
        monster.Rpc_Scream();
        float animLength = monster.GetCurrentAnimLength();

        _tickTimer = TickTimer.CreateFromSeconds(Runner, animLength);   // �ش� �ð��� ���� ���� ���� ����

        // Spawn
        //monster.StartCoroutine(monster.SpawnAfterAnim(animLength));
    }
    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner))     // _tickTimer�� �ش� �ð���ŭ ������ true�� �ȴ�
        {
            // �߰� phase�� ��ȯ
            monster.FSM.ChangePhase<Grita_Phase_Chase>(); // 0�� ���� Run�� ����
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Scream Exit");
        monster.IsScream = false;
    }
}

