

// 모든 상태머신의 부모(플레이어, NPC 등)
public abstract class StateMachine
{
    // 현재 상태를 가지고 있어야한다
    protected IState currentState;

    // 확장성을 고려하면 BaseController를 선언하는게 맞지만
    // 사용하는 곳에서 ((PlayerController) controller).PlayerAnimation 처럼 쓸때마다 형변환해야하는 문제가 생길 수 있음
    // PlayerStateMachine에 PlayerController를 바로 선언할 수도 있지만 이때는 확장성이 줄어든다는 단점이 있다
    protected BaseController controller;

    // 현재의 상태를 종료하고 새로운 상태를 실행
    public void ChangeState(IState newState)
    {
        if (currentState?.GetType() == newState?.GetType()) return;

        /// 가장 처음으로 들어가는 State는 IdleState
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    /// <summary>
    /// 현재 상태의 입력값, 조건에 따라 다른 상태로 이동할 것인지 판단
    /// 하지만 네트워크에서 입력을 받아 업데이트하므로 호출하지 않는다
    /// </summary>
    public void HandleInput()
    {
        // 현재 상태에서 Input값을 확인한다
        currentState?.HandleInput();
    }

    // 생명주기함수 아니다
    public void OnUpdate(NetworkInputData data)
    {
        currentState?.OnUpdate(data);
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}