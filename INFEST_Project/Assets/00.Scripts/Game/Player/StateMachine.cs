

// ��� ���¸ӽ��� �θ�(�÷��̾�, NPC ��)
public abstract class StateMachine
{
    // ���� ���¸� ������ �־���Ѵ�
    protected IState currentState;

    // Ȯ�强�� ����ϸ� BaseController�� �����ϴ°� ������
    // ����ϴ� ������ ((PlayerController) controller).PlayerAnimation ó�� �������� ����ȯ�ؾ��ϴ� ������ ���� �� ����
    // PlayerStateMachine�� PlayerController�� �ٷ� ������ ���� ������ �̶��� Ȯ�强�� �پ��ٴ� ������ �ִ�
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

    /// <summary>
    /// ���� ������ �Է°�, ���ǿ� ���� �ٸ� ���·� �̵��� ������ �Ǵ�
    /// ������ ��Ʈ��ũ���� �Է��� �޾� ������Ʈ�ϹǷ� ȣ������ �ʴ´�
    /// </summary>
    public void HandleInput()
    {
        // ���� ���¿��� Input���� Ȯ���Ѵ�
        currentState?.HandleInput();
    }

    // �����ֱ��Լ� �ƴϴ�
    public void OnUpdate(NetworkInputData data)
    {
        currentState?.OnUpdate(data);
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}