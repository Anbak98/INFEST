using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Phase_Wander : MonsterPhase<Monster_Grita>
{
    //public TickTimer screemCooldownTickTimer;
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.IsWonderPhase = true;

        // 0��¥�� ����� Expired�� true�� ���� 1�� ����
        //screemCooldownTickTimer = TickTimer.CreateFromSeconds(Runner, 0f);
    }

    // �÷��̾ �������� �̳��� ������ Scream���� ���¸� �ٲ���Ѵ�
    // Idle, Walk state ��� ����ǹǷ� �̰��� �ۼ��Ѵ�..?


    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.IsFindPlayer() && !monster.IsDead)
        {
            monster.FSM.ChangePhase<Grita_Phase_Chase>();
        }
    }

    public override void MachineExit()
    {
        base.MachineExit();
        monster.IsWonderPhase = false;
    }
}
