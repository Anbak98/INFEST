using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Idle : MonsterStateNetworkBehaviour
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 7);
    }
    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner)) // ���� �ð� �ʰ��ϸ� true ����
        {
            phase.ChangeState<Grita_Walk>(); // state ��ü

            //// ����
            //monster.FSM.ChangePhase<PJ_HI_II_DeadPhase>(); //phase ��ü
            //monster.MovementSpeed = monster.info.SpeedMove; //��ȹ���� �ִ� �޸��� �ӵ�
            //monster.AIPathing.SetDestination(monster.target.transform.position); // �߰�
            //// Monsterinfo���� �ҷ���



        }
    }
}
