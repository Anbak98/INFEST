using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// 메인카메라의 이동 회전을 다룬다
/// scope카메라를 저장한다
/// </summary>
public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] private Camera _scopeCam;          // scope 전용 카메라
    [SerializeField] private Transform cameraHolder;    // 카메라 부모 (X축 회전만 담당)
    [SerializeField] private float _sensitivity = 5f;   // 이동에 적용할 민감도

    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1인칭 카메라


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
        //마우스 이동량 (InputSystem)
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = (yRotation + mouseDelta.x) * _sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * _sensitivity * Time.deltaTime;

        // 좌우 회전 (플레이어)
        transform.Rotate(Vector3.up * mouseX);

        // 상하 회전 (카메라 홀더)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // 상하 회전 제한
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
