using Cinemachine;
using Fusion;
using Fusion.Addons.SimpleKCC;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// 카메라의 이동 회전을 다룬다
/// 회전은 상태 변화에 영향을 미치지 않으므로 StateMachine으로부터 독립적으로 작성
/// </summary>
public class PlayerCameraHandler : NetworkBehaviour
{
    [SerializeField] private Camera _scopeCam;          // scope 전용 카메라
    [SerializeField] private Transform _cameraHolder;    // 카메라 부모 (X축 회전만 담당)
    public float _sensitivity = 0.3f;   // 이동에 적용할 민감도
    [SerializeField] private Transform _parentTransform;

    // 마우스의 회전값
    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1인칭 카메라
    public bool isMenu;
    public PlayerStatHandler statHandler;

    // 관전모드 카메라 검색을 줄이기 위해
    public CinemachineVirtualCamera virtualCamera;

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

        if (statHandler.CurHealth <= 0) return;

        if (Runner.TryGetInputForPlayer(Object.InputAuthority, out NetworkInputData input) == true)
        {
            // Apply look rotation delta. This propagates to Transform component immediately.
        //    if (HasStateAuthority)
        //{
        //    if (GetInput(out NetworkInputData data))
        ////    {
        //    Vector2 mouseDelta = input.lookDelta;

        //    yRotation = mouseDelta.x * _sensitivity * Time.deltaTime;
        //    xRotation -= mouseDelta.y * _sensitivity * Time.deltaTime;
        //    xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        //    _parentTransform.Rotate(yRotation * Vector3.up);
        //    _cameraHolder.localEulerAngles = new Vector3(xRotation, 0f, 0f); // X축 회전만 적용
            _simpleKCC.AddLookRotation(new Vector3(-input.lookDelta.y * 0.2f, input.lookDelta.x * 0.2f, 0));

            Vector2 pitchRotation = _simpleKCC.GetLookRotation(true, false);
            _cameraHolder.localRotation = Quaternion.Euler(pitchRotation);
        }
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
