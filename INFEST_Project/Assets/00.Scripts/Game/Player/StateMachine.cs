using Unity.VisualScripting;


// ��� ���¸ӽ��� �θ�(�÷��̾�, NPC ��)
public abstract class StateMachine
{
    // ���� ���¸� ������ �־���Ѵ�
    protected IState currentState;

    protected BaseController controller;

    // ������ ���¸� �����ϰ� ���ο� ���¸� ����
    public void ChangeState(IState newState)
    {
        if (currentState?.GetType() == newState?.GetType()) return;

        /// ���� ó������ ���� State�� IdleState
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    // �����ֱ��Լ� �ƴϴ�
    public void OnUpdate(NetworkInputData data)
    {
        currentState?.OnUpdate(data);
    }

    public void PhysicsUpdate(NetworkInputData data)
    {
        currentState?.PhysicsUpdate(data);
    }
}