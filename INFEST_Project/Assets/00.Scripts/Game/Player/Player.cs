using Fusion;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    //[SerializeField] private Ball _prefabBall;
    // ��ư ������ ������ ���� Ÿ�̸Ӹ� �缳��
    [Networked] private TickTimer delay { get; set; }

    // 
    private NetworkCharacterController _cc;
    private Vector3 _forward = Vector3.forward;

    //[SerializeField] private PhysxBall _prefabPhysxBall;

    // ��Ʈ��ũ �Ӽ��� ������ �� Fusion�� ������ get �� set ������ ����� ���� �ڵ�� ��ü�Ͽ� ��Ʈ��ũ ���¿� ����
    // �̴� ���ø����̼��� �̷��� ����� ����Ͽ� �Ӽ� ���� ��ȭ�� ó���� �� ������
    // ������ setter �޼ҵ带 ����� ���� ���ÿ����� �۵��Ѵٴ� ���� �ǹ�
    [Networked] public bool spawnedProjectile { get; set; }

    private ChangeDetector _changeDetector;

    public Material _material;

    // RPC(���� ���ν��� ȣ��)�� Ư�� �̺�Ʈ�� ��Ʈ��ũ Ŭ���̾�Ʈ ���� �����ϱ⿡ �̻����Դϴ�.
    // �ݸ鿡, [Networked] �Ӽ��� ���������� ���ϴ� ���¸� �����ϴ� �� ������ �⺻ �ַ���Դϴ�.
    //[Networked] public string playerName { get; set; }
    //[Networked] public Color playerColor { get; set; }

    private TMP_Text _messages;


    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;

        /// Player�� ���� PlayerColor ��ũ��Ʈ�� MeshRenderer�� �����Ͽ� material�� �����´�
        _material = GetComponentInChildren<MeshRenderer>().material;
    }

    public override void FixedUpdateNetwork()
    {
        /// GetInput: OnInput�� �ִ� ������ �����´�
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);

            if (data.direction.sqrMagnitude > 0)
                _forward = data.direction;  //  ������ �̵� ������ �����ϰ� �̸� ���� �� �������� ���

            // ��Ʈ��ũ ��ü�� StateAuthority(ȣ��Ʈ)�� ������ �� �ֱ� ������ StateAuthority�� ���� Ȯ���� �ʿ�
            // ȣ��Ʈ������ ����ǰ� Ŭ���̾�Ʈ������ �������� �ʴ´�
            if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
            {
                // ���콺 ��Ŭ��(����)
                if (data.buttons.IsSet(NetworkInputData.BUTTON_ATTACK))
                {
                    Debug.Log("����");

                    //delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    //Runner.Spawn(_prefabBall,
                    //transform.position + _forward,
                    //Quaternion.LookRotation(_forward),
                    //  Object.InputAuthority, (runner, o) =>
                    //  {
                    //      // Initialize the Ball before synchronizing it
                    //      o.GetComponent<Ball>().Init();
                    //  });
                    //spawnedProjectile = !spawnedProjectile;
                }
                // PhyscBall.Init() �޼ҵ带 ����Ͽ� ������ ȣ���ϰ� �ӵ�(������ �����⿡ ���� ���)�� ����
                
                // ���콺 ��Ŭ��(ZOOM)
                else if (data.buttons.IsSet(NetworkInputData.BUTTON_ZOOM))
                {
                    Debug.Log("����");

                    //delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    //Runner.Spawn(_prefabPhysxBall,
                    //  transform.position + _forward,
                    //  Quaternion.LookRotation(_forward),
                    //  Object.InputAuthority,
                    //  (runner, o) =>
                    //  {
                    //      o.GetComponent<PhysxBall>().Init(10 * _forward);
                    //  });
                    //spawnedProjectile = !spawnedProjectile;
                }
                // Runner.Spawn()�� ȣ���� �� spawnedProjectile �Ӽ��� ��� �Ͽ� �ݹ��� Ʈ����
                //spawnedProjectile = !spawnedProjectile;
            }
        }
    }

    private void Update()
    {
        // Object.HasInputAuthority�� Ȯ���Ѵ�: ��� Ŭ���̾�Ʈ���� ��������� �� �÷��̾ �����ϴ� Ŭ���̾�Ʈ�� RPC�� ȣ���ؾ� �ϱ� ����
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        {
            RPC_SendMessage("Hey Mate!");
        }
    }
    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }
    /// <summary>
    /// ���������� ChangeDetector�� ȣ���� ���� ��Ʈ��ũȭ�� �Ӽ��� �߻��� ��� ���� ������ �ݺ�
    /// Render()�� Unity�� Update()ó�� Fusion�� �� �����Ӹ��� ������ �������� �ڵ����� ȣ���ϴ� �Լ���
    /// </summary>
    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(spawnedProjectile):
                    /// PlayerColor ��ũ��Ʈ�� MeshRenderer�� �����Ͽ� material�� ���� ����
                    _material.color = Color.white;
                    break;
            }
        }
        /// PlayerColor ��ũ��Ʈ�� MeshRenderer�� �����Ͽ� material�� ���� ����
        _material.color = Color.Lerp(_material.color, Color.blue, Time.deltaTime);
    }

    /// <summary>
    /// RPC ���� �޼����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="color"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        RPC_RelayMessage(message, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string message, PlayerRef messageSource)
    {
        if (_messages == null)
            _messages = FindObjectOfType<TMP_Text>();

        if (messageSource == Runner.LocalPlayer)
        {
            message = $"You said: {message}\n";
        }
        else
        {
            message = $"Some other player said: {message}\n";
        }

        _messages.text += message;
    }
}