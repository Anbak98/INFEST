using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// ����ī�޶��� �̵� ȸ���� �ٷ��
/// scopeī�޶� �����Ѵ�
/// </summary>
public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] private Camera _scopeCam;          // scope ���� ī�޶�
    [SerializeField] private Transform cameraHolder;    // ī�޶� �θ� (X�� ȸ���� ���)
    [SerializeField] private float _sensitivity = 5f;   // �̵��� ������ �ΰ���

    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1��Ī ī�޶�


    private void Awake()
    {
        InitCamera();
    }

    private void Start()
    {
    }

    private void Update()
    {
        RotateCamera();
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

    private void RotateCamera()
    {
        //���콺 �̵��� (InputSystem)
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = (yRotation + mouseDelta.x) * _sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * _sensitivity * Time.deltaTime;

        // �¿� ȸ�� (�÷��̾�)
        transform.Rotate(Vector3.up * mouseX);

        // ���� ȸ�� (ī�޶� Ȧ��)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // ���� ȸ�� ����
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
