using Cinemachine;
using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>
/// ī�޶��� �̵� ȸ���� �ٷ��
/// ȸ���� ���� ��ȭ�� ������ ��ġ�� �����Ƿ� StateMachine���κ��� ���������� �ۼ�
/// </summary>
public class PlayerCameraHandler : NetworkBehaviour
{
    [SerializeField] private Camera _scopeCam;          // scope ���� ī�޶�
    [SerializeField] private Image _scopeImage;
    [SerializeField] private Transform _cameraHolder;    // ī�޶� �θ� (X�� ȸ���� ���)
    public float _sensitivity = 1f;   // �̵��� ������ �ΰ���
    [SerializeField] private Transform _parentTransform;

    // ���콺�� ȸ����
    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1��Ī ī�޶�
    public bool isMenu;
    [SerializeField] private Player _player;

    // �������
    public CinemachineVirtualCamera firstPersonCamera;      // �ڽ��� ���÷��� ī�޶�
    public CinemachineVirtualCamera spectatorCamera;    // �������� ����� ī�޶�


    private void Awake()
    {
        InitCamera();
        isMenu = false;
    }

    public Camera GetCamera(bool scopeCam)
    {
        return scopeCam ? _scopeCam : _mainCam;
    }

    private void InitCamera()
    {
        _mainCam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    [SerializeField] private SimpleKCC _simpleKCC;

    public override void FixedUpdateNetwork()
    {

        base.FixedUpdateNetwork();

        if (isMenu) return;

        if (_player.statHandler.CurHealth <= 0) return;

        if (Runner.TryGetInputForPlayer(Object.InputAuthority, out NetworkInputData input) == true)
        {
            _simpleKCC.AddLookRotation(new Vector3(-input.lookDelta.y * _sensitivity * Runner.DeltaTime, input.lookDelta.x * _sensitivity * Runner.DeltaTime, 0));

            Vector2 pitchRotation = _simpleKCC.GetLookRotation(true, false);
            _cameraHolder.localRotation = Quaternion.Euler(pitchRotation);
        }

        _scopeImage.rectTransform.position = _mainCam.WorldToScreenPoint(_player.inventory.equippedWeapon.FPSWeapon.aimPoint.transform.position);
        _scopeImage.gameObject.SetActive(_player.inventory.equippedWeapon.IsAiming);
        _scopeCam.enabled = _player.inventory.equippedWeapon.IsAiming;
    }
    public void LateUpdate()
    {
        // Only InputAuthority needs to update camera.
        if (Object == null || Object.HasInputAuthority == false)
            return;

        // Update camera pivot and transfer properties from camera handle to Main Camera.
        // LateUpdate() is called after all Render() calls - the character is already interpolated.

        Vector2 pitchRotation = _simpleKCC.GetLookRotation(true, false);
        _cameraHolder.localRotation = Quaternion.Euler(pitchRotation);
    }

    public Vector3 GetCameraForwardOnXZ()
    {
        //Vector3 camForward = _mainCam.transform.forward;
        Vector3 camForward = transform.forward;
        camForward.y = 0f;
        return camForward.normalized;
    }

    public Vector3 GetCameraRightOnXZ()
    {
        Vector3 camRight = transform.right;
        camRight.y = 0f;
        return camRight.normalized;
    }
}
