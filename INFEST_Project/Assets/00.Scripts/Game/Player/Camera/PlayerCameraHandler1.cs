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
    [SerializeField] private Transform _parentTransform;

    // ���콺�� ȸ����
    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    // ȸ�� �ִϸ��̼��� �Ķ����
    public float lookX { get; set; } = 0f;
    public float lookY { get; set; } = 0f;
    private float lerpSpeed = 5f; // ���� �ӵ�(�������� ������ ��ȭ)


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
            Vector2 mouseDelta = data.lookDelta;

            float mouseX = mouseDelta.x * _sensitivity * Time.deltaTime;
            float mouseY = mouseDelta.y * _sensitivity * Time.deltaTime;

            // �¿� ȸ�� (�÷��̾�)
            _parentTransform.Rotate(Vector3.up * mouseX);

            // ���� ȸ��
            //if (_cameraHolder.rotation.eulerAngles.x > 80f)
            //    return;
            //else if (_cameraHolder.rotation.eulerAngles.x < -80f)
            //    return;
            //else
                _cameraHolder.Rotate(Vector3.right * -mouseY);

            //xRotation -= mouseY;
            //xRotation = Mathf.Clamp(xRotation, -80f, 80f); // ���� ȸ�� ����
            //_cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // lookX, lookY�� ���ؼ� �����Ѵ�
            lookX = Mathf.Lerp(lookX, data.lookDelta.x, lerpSpeed * Time.deltaTime);
            lookX = Mathf.Clamp(lookX, -1f, 1f);    // ���� ����
            lookY = Vector3.Dot(transform.forward, Vector3.up);
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
        Vector3 camRight = transform.right;
        camRight.y = 0f;
        return camRight.normalized;
    }
}
