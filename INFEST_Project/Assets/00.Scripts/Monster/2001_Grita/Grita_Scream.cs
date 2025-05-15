using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave�� �ƴϴ�
// Scream���¿��� �� ��
// �Ҹ� ������
public class Grita_Scream : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wander>
{
    public TickTimer _animTickTimer;

    public override void Enter()
    {
        base.Enter();
        // 
        if (!monster.screemCooldownTickTimer.Expired(Runner))
        {
            monster.FSM.ChangePhase<Grita_Phase_Chase>(); // 0�� ���� Run�� ����
            return;
        }

        // Scream
        monster.IsScream = true;
        monster.IsCooltimeCharged = false;  // ��� ������
        monster.ScreamCount++;
        Debug.Log("Scream Enter");
        monster.Rpc_Scream();
        float animLength = monster.GetCurrentAnimLength();

        _animTickTimer = TickTimer.CreateFromSeconds(Runner, animLength);

        // Wave�� ���۵ȴ�(���ѻ����Ǵ� ����)
        //monster.spawner.SpawnMonsterOnWave(monster.target.transform);
    }
    public override void Execute()
    {
        base.Execute();
                
        if (_animTickTimer.Expired(Runner))     // _tickTimer�� �ش� �ð���ŭ ������ true�� �ȴ�
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

// Wave �ܰ�(Wave phase�ʹ� �ٸ���)

