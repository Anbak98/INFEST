using UnityEngine;

public class Bowmeter_RunWave : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();

        monster.AIPathing.SetDestination(monster.target.position);

        // ���� ��ΰ� ������ �ʾҰų� ������ ���
        if (monster.AIPathing.enabled && !monster.AIPathing.pathPending)
        {

            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                Debug.Log(monster.target.position + "  " + transform.position);
                Debug.Log(monster.AIPathing.remainingDistance + "  " + monster.AIPathing.stoppingDistance);
                phase.ChangeState<Bowmeter_AttackWave>();
            }
            //else if (monster.AIPathing.remainingDistance > 10f)
            //{
            //    monster.FSM.ChangePhase<Stacker_Phase_Wonder>();
            //}
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
