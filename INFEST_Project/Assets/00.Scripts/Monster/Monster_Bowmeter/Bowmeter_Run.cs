using UnityEngine;

public class Bowmeter_Run : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    //Transform _target;

    public override void Enter()
    {
        base.Enter();
        //_target = monster.target;
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();
        monster.AIPathing.SetDestination(monster.target.position);

        float distance = monster.AIPathing.remainingDistance;

        if (distance <= 5f)
        {
            // Chase phase�� ���� ��� �Լ� ȣ��
            phase.CaculateAttackType(distance);

            switch (phase.nextPatternIndex)
            {
                case 0:
                    phase.ChangeState<Bowmeter_Run>(); break;
                case 1:
                    phase.ChangeState<Bowmeter_Pattern1>(); break;
                case 2:
                    phase.ChangeState<Bowmeter_Pattern2>(); break;
                case 3:
                    phase.ChangeState<Bowmeter_Pattern3>(); break;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
