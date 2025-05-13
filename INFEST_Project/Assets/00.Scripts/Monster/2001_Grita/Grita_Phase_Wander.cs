using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Phase_Wander : MonsterPhase<Monster_Grita>
{
    public TickTimer screemCooldownTickTimer;

    public override void MachineEnter()
    {
        base.MachineEnter();

        // 0��¥�� ����� Expired�� true�� ���� 1�� ����
        screemCooldownTickTimer = TickTimer.CreateFromSeconds(Runner, 0f);
    }

    // �÷��̾ �������� �̳��� ������ Scream���� ���¸� �ٲ���Ѵ�
    // Idle, Walk state ��� ����ǹǷ� �̰��� �ۼ��Ѵ�..?
    public override void MachineExecute()
    {


        base.MachineExecute();

        //if ()
        // FSM�� ����Ǹ� �⺻ Phase�� 0�� State�� ����
        // State���� ��ȯ�� State ������ �̷������
        // Phase�� ��ȯ�� ��𿡼� �̷������?
        // �ƹ����� �ϸ� �ȴ�
        // State���� �ٸ� Phase ���� State�� ��ȯ�ϰ� �ʹٸ� State������ ȣ���ϴ� ���� �´�
       

    }

}
