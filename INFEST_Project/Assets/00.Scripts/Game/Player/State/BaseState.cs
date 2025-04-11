using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    protected PlayerController controller;
    protected PlayerStateMachine stateMachine;

    public BaseState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    /// <summary>
    /// ���� ���� �� ȣ��
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// ���� ���� �� ȣ��
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// �Է� ó��
    /// </summary>
    public virtual void HandleInput(NetworkInputData input) { }

    /// <summary>
    /// �� FixedUpdateNetwork���� ȣ���
    /// </summary>
    public virtual void UpdateLogic() { }

    /// <summary>
    /// �ʿ��ϴٸ� ���º��� ���� ��굵 ����
    /// </summary>
    public virtual void UpdatePhysics() { }
}
