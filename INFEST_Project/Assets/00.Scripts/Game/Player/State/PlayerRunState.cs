using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // �ϴ� ���ڴ���. ���߿� PlayStatData.RunSpeedModifier ���� �߰��ؼ� ��,������ �ٲ۴�
        stateMachine.StatHandler.MoveSpeedModifier = 4;
        Debug.Log("Run���� ����");
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {

    }
}
