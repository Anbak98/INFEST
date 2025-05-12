using UnityEngine;

public class Stacker_RunWave : MonsterStateNetworkBehaviour<Monster_Stacker>
{
    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = monster.info.SpeedMoveWave;

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
                phase.ChangeState<Stacker_AttackWave>();
            }
            //else if (monster.AIPathing.remainingDistance > 10f)
            //{
            //    monster.FSM.ChangePhase<PJ_HI_WonderPhase>();
            //}
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.MovementSpeed = 0;

    }
}
