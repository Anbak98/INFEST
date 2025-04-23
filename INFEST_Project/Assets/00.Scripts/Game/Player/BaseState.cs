using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    public void Enter();    // ���� ����
    public void Exit();     // ���� ��
    public void HandleInput();  // ���ο� �Է°��� ������ �̺�Ʈ�� �߰�, ����
    public void Update();   // ���� ������Ʈ
    public void PhysicsUpdate();    // ���� ������Ʈ 
}

/// <summary>
/// ��� ������ ��� Ŭ����
/// </summary>
public abstract class BaseState : IState
{
    protected BaseController controller;    // PlayerController, MonsterController
    protected StateMachine stateMachine;    // PlayerStateMachine, MonsterStateMachine

    public BaseState(BaseController controller, StateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    /// <summary>
    /// ���� ���� �� ȣ��
    /// </summary>
    public virtual void Enter() 
    {
    }

    /// <summary>
    /// ���� ���� �� ȣ��
    /// </summary>
    public virtual void Exit() 
    {
    }

    /// <summary>
    /// ���ο� �Է°��� ������ �̺�Ʈ�� �߰�, ����
    /// </summary>
    public virtual void HandleInput() 
    {
    }


    public virtual void Update()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    ///// <summary>
    ///// �� FixedUpdateNetwork���� ȣ���
    ///// </summary>
    //public virtual void UpdateLogic() { }

    ///// <summary>
    ///// �ʿ��ϴٸ� ���º��� ���� ��굵 ����
    ///// </summary>
}
