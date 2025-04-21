using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSitReloadState : PlayerSitState
{
    private bool prevIsReloading = false;

    public PlayerSitReloadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("SitReload���� ����");
        base.Enter();

        // Sit && Reload
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
        PlayerWaddle();

        // false �� true�� �ٲ�� ������ ���� (��, �Է��� �� ���� �� ����)
        if (!prevIsReloading && currentIsReloading)
        {
            Reload();
        }

        // reloading �������� ���� ������
        if (!currentIsReloading)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }

        prevIsReloading = currentIsReloading; // ���� frame�� ���� ����
    }
}
