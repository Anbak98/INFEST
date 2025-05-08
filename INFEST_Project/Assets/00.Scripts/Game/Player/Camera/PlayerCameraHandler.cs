using Cinemachine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
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
    [Networked] public float xRotation { get; set; } = 0f;
    [Networked] public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1��Ī ī�޶�
    public bool isMenu;
    public PlayerStatHandler statHandler;
    public Transform hand;
    private Vector3 startHand;
    // ������� ī�޶� �˻��� ���̱� ����
    public CinemachineVirtualCamera virtualCamera;

    // ī�޶� ��鸲
    private CinemachineBasicMultiChannelPerlin _noise; // ������ ������Ʈ
    private float shakeTimer;
    private float amplitude;
    private float frequency;

    private void Awake()
    {
        InitCamera();
        isMenu = false;
        _noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        startHand = hand.localPosition;
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

        shakeTimer -= Time.deltaTime;

        if (shakeTimer > 0)
        {
            Vector3 shakeOffset = new Vector3(
                        Mathf.PerlinNoise(Time.time * frequency, 0f) - 0.5f,
                        Mathf.PerlinNoise(0f, Time.time * frequency) - 0.5f,
                        0f
                        ) * amplitude;
        }
        else
        {
            _noise.m_AmplitudeGain = 0f;
            _noise.m_FrequencyGain = 0f;

            hand.localPosition = startHand;

        }

        if (isMenu) return;

        if (statHandler.CurrentHealth <= 0) return;

            if (GetInput(out NetworkInputData data))
            {
                Vector2 mouseDelta = data.lookDelta;

                float mouseX = (yRotation + mouseDelta.x) * _sensitivity * Time.deltaTime;
                float mouseY = mouseDelta.y * _sensitivity * Time.deltaTime;

                // �¿� ȸ�� (�÷��̾�)
                _parentTransform.Rotate(Vector3.up * mouseX);

                // ���� ȸ��
                //if (_cameraHolder.rotation.eulerAngles.x > 80f)
                //    return;
                //else if (_cameraHolder.rotation.eulerAngles.x < -80f)
                //    return;
                //else
                //  _cameraHolder.Rotate(Vector3.right * -mouseY);

                // ���� ȸ�� (ī�޶� Ȧ����)
                xRotation -= mouseY; // ���� �̵��ϸ� ����, �Ʒ��� �̵��ϸ� ���
                xRotation = Mathf.Clamp(xRotation, -80f, 80f);

                _cameraHolder.localEulerAngles = new Vector3(xRotation, 0f, 0f); // X�� ȸ���� ����
            }
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

    public void Shake(float duration, float amp, float freq)
    {
        amplitude = amp;
        frequency = freq;
        shakeTimer = duration;

        _noise.m_AmplitudeGain = amplitude;
        _noise.m_FrequencyGain = frequency;

        Vector3 shakeOffset = new Vector3(
            Mathf.PerlinNoise(Time.time * frequency, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * frequency) - 0.5f,
            0f
            ) * amplitude;

        hand.localPosition = startHand + shakeOffset;
    }
}
