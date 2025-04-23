using Unity.VisualScripting;


// 모든 상태머신의 부모(플레이어, NPC 등)
public abstract class StateMachine
{
    // 현재 상태를 가지고 있어야한다
    protected IState currentState;

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

    // 생명주기함수 아니다
    public void OnUpdate(NetworkInputData data)
    {
        currentState?.OnUpdate(data);
    }

    public void PhysicsUpdate(NetworkInputData data)
    {
        currentState?.PhysicsUpdate(data);
    }
}