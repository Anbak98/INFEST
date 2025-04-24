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

        //StartAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }


    public override void OnUpdate(NetworkInputData data)
    {
        // ���� �ִϸ��̼� 1ȸ ���� �� �ǰ� �ٷ� �������ϴµ�... �̸� ��� Ȯ��?
        bool currentIsReloading = data.isReloading; // �ܺο��� bool ��������
        // �ִϸ��̼� ����ð����� �����ؾ��Ѵ�



        // �̵��ϸ鼭 ������ �����ϴ�
        PlayerMove(data);

        // false �� true�� �ٲ�� ������ ���� (��, �Է��� �� ���� �� ����)
        if (!prevIsReloading && data.isReloading)
        {
            Reload(data);
            return;
        }

        // reloading �������� ���� ������
        if (!data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        prevIsReloading = data.isReloading; // ���� frame�� ���� ����
    }

    // ���� �������� �ٽ� idle�� �ǵ��ư���
}
