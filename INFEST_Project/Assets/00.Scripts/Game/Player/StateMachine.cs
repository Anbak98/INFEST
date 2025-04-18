

// 모든 상태머신의 부모(플레이어, NPC 등)
public abstract class StateMachine
{
    // 현재 상태를 가지고 있어야한다
    protected IState currentState;

    // 확장성을 고려하면 BaseController를 선언하는게 맞지만
    // 사용하는 곳에서 ((PlayerController) controller).PlayerAnimation 처럼 쓸때마다 형변환해야하는 문제가 생길 수 있음
    // PlayerStateMachine에 PlayerController를 바로 선언할 수도 있지만 이때는 확장성이 줄어든다는 단점이 있다
    protected BaseController controller;

    // 확장성과 형변환 사이에서 고민한 결과 제네릭을 사용하기로 결정
    //public T GetController<T>() where T : BaseController => controller as T;

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
    /// 현재 상태, 입력값, 조건에 따라 다른 상태를 판단
    /// </summary>
    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    // 생명주기함수 아니다
    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}