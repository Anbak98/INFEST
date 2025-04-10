using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerController���� ���� ������ �������� ���¸� ������Ʈ(���� Ȥ�� ��ȭ)
/// </summary>
public class PlayerStateMachine : StateMachine
{
    public BaseState currentState;

    public void ChangeState(BaseState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        if (currentState != null)
            currentState.Enter();
    }

    public void Update()
    {
        //currentState?.Update();
    }

    /// <summary>
    /// ���� ������Ʈ�� �� ���̴�
    /// ��Ʈ��ũ�κ��� ���� �޴� FixedUpdateNetwork�� �ƴԿ� �����Ѵ�
    /// 
    /// </summary>
    public void FixedUpdate()
    {
        //currentState?.FixedUpdate();

        // ���¸� 

    }
}
