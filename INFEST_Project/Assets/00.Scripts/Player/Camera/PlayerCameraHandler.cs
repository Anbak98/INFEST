using Cinemachine;
using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>
/// 카메라의 이동 회전을 다룬다
/// 회전은 상태 변화에 영향을 미치지 않으므로 StateMachine으로부터 독립적으로 작성
/// </summary>
public class PlayerCameraHandler : NetworkBehaviour
{
    [SerializeField] private Camera _scopeCam;          // scope 전용 카메라
    [SerializeField] private Image _scopeImage;
    [SerializeField] private Transform _cameraHolder;    // 카메라 부모 (X축 회전만 담당)
    public float _sensitivity = 1f;   // 이동에 적용할 민감도
    [SerializeField] private Transform _parentTransform;

    // 마우스의 회전값
    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1인칭 카메라
    public bool isMenu;
    [SerializeField] private Player _player;

    // 관전모드
    public CinemachineVirtualCamera firstPersonCamera;      // 자신의 인플레이 카메라
    public CinemachineVirtualCamera spectatorCamera;    // 관전모드로 사용할 카메라


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
