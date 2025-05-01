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
    public float _sensitivity = 5f;   // 이동에 적용할 민감도
    [SerializeField] private Transform _parentTransform;

    // 마우스의 회전값

    [Networked] public float xRotation { get; set; } = 0f;
    [Networked] public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1인칭 카메라
    public bool isMenu;

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

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (isMenu) return;

        if (HasStateAuthority)
        {            
            if (GetInput(out NetworkInputData data))
            {

                Vector2 mouseDelta = data.lookDelta;

                float mouseX = (yRotation + mouseDelta.x) * _sensitivity * Time.deltaTime;
                float mouseY = mouseDelta.y * _sensitivity * Time.deltaTime;

                // 좌우 회전 (플레이어)
                _parentTransform.Rotate(Vector3.up * mouseX);

                // 상하 회전
                //if (_cameraHolder.rotation.eulerAngles.x > 80f)
                //    return;
                //else if (_cameraHolder.rotation.eulerAngles.x < -80f)
                //    return;
                //else
                //  _cameraHolder.Rotate(Vector3.right * -mouseY);

                // 상하 회전 (카메라 홀더만)
                xRotation -= mouseY; // 위로 이동하면 음수, 아래로 이동하면 양수
                xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            }
        }

        _cameraHolder.localEulerAngles = new Vector3(xRotation, 0f, 0f); // X축 회전만 적용
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

    public void LockCamera(bool active)
    {
        isMenu = active;

        if (active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
}
