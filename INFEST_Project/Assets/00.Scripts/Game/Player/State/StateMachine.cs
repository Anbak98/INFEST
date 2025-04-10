using Unity.VisualScripting;


public interface IState
{
    public void Enter();    // 상태 진입
    public void Exit();     // 상태 끝
    public void HandleInput();  // 새로운 입력값이 들어오면 이벤트를 추가, 삭제
    public void Update();   // 상태 업데이트
    public void PhysicsUpdate();    // 물리 업데이트 
}

// 모든 상태머신의 부모
public abstract class StateMachine
{
    protected IState currentState;

    // 함수들은 굳이 재정의 안하고 base에 있는 것을 써도 충분하다
    public void ChangeState(IState state)
    {
        currentState?.Exit();   /// 가장 처음으로 들어가는 State는 IdleState
        currentState = state;
        currentState?.Enter();
    }

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