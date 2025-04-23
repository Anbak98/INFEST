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
        //StartAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // �̵��ϸ鼭 ������ �����ϴ�
        PlayerWaddle(data);

        // false �� true�� �ٲ�� ������ ���� (��, �Է��� �� ���� �� ����)
        if (!prevIsReloading && data.isReloading)
        {
            SitReload(data);
        }

        // reloading �������� ���� ������
        if (!data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }

        prevIsReloading = data.isReloading; // ���� frame�� ���� ����
    }
}
