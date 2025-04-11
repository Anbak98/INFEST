using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerController에서 받은 정보를 바탕으로 상태를 업데이트(유지 혹은 변화)
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
    /// 상태 업데이트를 할 뿐이다
    /// 네트워크로부터 직접 받는 FixedUpdateNetwork가 아님에 주의한다
    /// 
    /// </summary>
    public void FixedUpdate()
    {
        //currentState?.FixedUpdate();

        // 상태를 

    }
}
