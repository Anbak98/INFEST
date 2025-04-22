using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReloadState : PlayerBaseState
{
    private bool prevIsReloading = false;

    public PlayerReloadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {

    }
    public override void Enter()
    {
        Debug.Log("Reload���� ����");
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }


    public override void Update()
    {
        // ���� �ִϸ��̼� 1ȸ ���� �� �ǰ� �ٷ� �������ϴµ�... �̸� ��� Ȯ��?
        bool currentIsReloading = stateMachine.InputHandler.GetIsReloading(); // �ܺο��� bool ��������

        // �̵��ϸ鼭 ������ �����ϴ�
        PlayerMove();

        // false �� true�� �ٲ�� ������ ���� (��, �Է��� �� ���� �� ����)
        if (!prevIsReloading && currentIsReloading)
        {
            Reload();
        }

        // reloading �������� ���� ������
        if (!currentIsReloading)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        prevIsReloading = currentIsReloading; // ���� frame�� ���� ����
    }

    // ���� �������� �ٽ� idle�� �ǵ��ư���
}
