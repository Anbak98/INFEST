using Fusion;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float _sensitivity = 5f;   // 이동에 적용할 민감도

    // 마우스의 회전값
    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1인칭 카메라


    private void Awake()
    {
        InitCamera();
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

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        // 권한설정을 하면 host만 내부에 진입한다
        //if (HasInputAuthority)
        //{
            if (GetInput(out NetworkInputData data))
            {
                Debug.LogFormat($"{gameObject.name}의 카메라 FixedUpdate"); // 어느 controller가 들어오는가?


                Vector2 mouseDelta = data.lookDelta;

                float mouseX = (yRotation + mouseDelta.x) * _sensitivity * Time.deltaTime;
                float mouseY = mouseDelta.y * _sensitivity * Time.deltaTime;

                // 좌우 회전 (플레이어)
                transform.Rotate(Vector3.up * mouseX);

                // 상하 회전 (카메라 홀더)
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -80f, 80f); // 상하 회전 제한
                _cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
        //}
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
        Vector3 camRight = _mainCam.transform.right;
        camRight.y = 0f;
        return camRight.normalized;
    }
}
