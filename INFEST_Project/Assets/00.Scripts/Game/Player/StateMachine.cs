

// ��� ���¸ӽ��� �θ�(�÷��̾�, NPC ��)
public abstract class StateMachine
{
    // ���� ���¸� ������ �־���Ѵ�
    protected IState currentState;

    // Ȯ�强�� ����ϸ� BaseController�� �����ϴ°� ������
    // ����ϴ� ������ ((PlayerController) controller).PlayerAnimation ó�� �������� ����ȯ�ؾ��ϴ� ������ ���� �� ����
    // PlayerStateMachine�� PlayerController�� �ٷ� ������ ���� ������ �̶��� Ȯ�强�� �پ��ٴ� ������ �ִ�
    protected BaseController controller;

    // Ȯ�强�� ����ȯ ���̿��� ����� ��� ���׸��� ����ϱ�� ����
    //public T GetController<T>() where T : BaseController => controller as T;

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
    /// ���� ����, �Է°�, ���ǿ� ���� �ٸ� ���¸� �Ǵ�
    /// </summary>
    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    // �����ֱ��Լ� �ƴϴ�
    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}