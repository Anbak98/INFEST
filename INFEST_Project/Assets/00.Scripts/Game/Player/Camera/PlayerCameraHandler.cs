using Fusion;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float _sensitivity = 5f;   // �̵��� ������ �ΰ���

    // ���콺�� ȸ����
    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1��Ī ī�޶�


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

        // ���Ѽ����� �ϸ� host�� ���ο� �����Ѵ�
        //if (HasInputAuthority)
        //{
            if (GetInput(out NetworkInputData data))
            {
                Debug.LogFormat($"{gameObject.name}�� ī�޶� FixedUpdate"); // ��� controller�� �����°�?


                Vector2 mouseDelta = data.lookDelta;

                float mouseX = (yRotation + mouseDelta.x) * _sensitivity * Time.deltaTime;
                float mouseY = mouseDelta.y * _sensitivity * Time.deltaTime;

                // �¿� ȸ�� (�÷��̾�)
                transform.Rotate(Vector3.up * mouseX);

                // ���� ȸ�� (ī�޶� Ȧ��)
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -80f, 80f); // ���� ȸ�� ����
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
