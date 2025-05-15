using Cinemachine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// ī�޶��� �̵� ȸ���� �ٷ��
/// ȸ���� ���� ��ȭ�� ������ ��ġ�� �����Ƿ� StateMachine���κ��� ���������� �ۼ�
/// </summary>
public class PlayerCameraHandler : NetworkBehaviour
{
    [SerializeField] private Camera _scopeCam;          // scope ���� ī�޶�
    [SerializeField] private Transform _cameraHolder;    // ī�޶� �θ� (X�� ȸ���� ���)
    public float _sensitivity = 5f;   // �̵��� ������ �ΰ���
    [SerializeField] private Transform _parentTransform;

    // ���콺�� ȸ����
    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1��Ī ī�޶�
    public bool isMenu;
    public PlayerStatHandler statHandler;

    // ������� ī�޶� �˻��� ���̱� ����
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

    public override void FixedUpdateNetwork()
    {

        base.FixedUpdateNetwork();

        if (isMenu) return;

        if (statHandler.CurHealth <= 0) return;

        if(HasStateAuthority)
        {
            if (GetInput(out NetworkInputData data))
            {
                Vector2 mouseDelta = data.lookDelta;

                yRotation = mouseDelta.x * _sensitivity * Time.deltaTime;
                xRotation -= mouseDelta.y * _sensitivity * Time.deltaTime;
                xRotation = Mathf.Clamp(xRotation, -80f, 80f);

                _parentTransform.Rotate(yRotation * Vector3.up);
                _cameraHolder.localEulerAngles = new Vector3(xRotation, 0f, 0f); // X�� ȸ���� ����
            }
        }
    }

    public override void Render()
    {
        base.Render();
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
