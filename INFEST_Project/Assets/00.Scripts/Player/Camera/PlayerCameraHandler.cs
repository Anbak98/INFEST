using Cinemachine;
using Fusion;
using Fusion.Addons.SimpleKCC;
using INFEST.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>
/// ī�޶��� �̵� ȸ���� �ٷ��
/// ȸ���� ���� ��ȭ�� ������ ��ġ�� �����Ƿ� StateMachine���κ��� ���������� �ۼ�
/// </summary>
public class PlayerCameraHandler : NetworkBehaviour
{
    public Player player;

    [SerializeField] private Camera _scopeCam;          // scope ���� ī�޶�
    [SerializeField] private Image _scopeImage;
    [SerializeField] private Transform _cameraHolder;    // ī�޶� �θ� (X�� ȸ���� ���)
    public float _sensitivity = 1f;   // �̵��� ������ �ΰ���
    [SerializeField] private Transform _parentTransform;

    // ���콺�� ȸ����
    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1��Ī ī�޶�
    public bool isMenu;
    [SerializeField] private Player _player;

    // �ٸ� �÷��̾ ��Ŀ�� �����(�������)
    public CinemachineVirtualCamera firstPersonCamera;      // �ڽ��� ���÷��� ī�޶�
    public CinemachineVirtualCamera spectatorCamera;    // curFocusingCam�� �����ϴ� ����� ī�޶�
    public List<CinemachineVirtualCamera> alivePlayerCameras = new List<CinemachineVirtualCamera>();
    public int currentPlayerIndex = 0;
    List<PlayerRef> playerRefs = new List<PlayerRef>();
    private int previousTime = -1;
    public CinemachineVirtualCamera curFocusingCam;   // ���� ��Ŀ�� ���
    public bool isFocusing = false; // �⺻�����δ� false


    private void Awake()
    {
        InitCamera();
        isMenu = false;
    }

    #region ī�޶� ���� ����

    public Camera GetCamera(bool scopeCam)
    {
        return scopeCam ? _scopeCam : _mainCam;
    }

    private void InitCamera()
    {
        _mainCam = Camera.main;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
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
    #endregion

    #region �ٸ� �÷��̾� ��Ŀ��(�������)
    public void FindAlivePlayers()
    {
        // ���� �������� �÷��̾� ������ ����
        playerRefs = NetworkGameManager.Instance.gamePlayers.GetPlayerRefs();
        // ���� ������ ����Ʈ ����
        alivePlayerCameras.Clear();
        // playerRefs�� �ִ� �÷��̾���� virtualCamera���� ��ġ�� ����
        foreach (var playerRef in playerRefs)
        {
            // NetworkObject ������ ���� Player ������Ʈ�� ����
            Player otherPlayer = NetworkGameManager.Instance.gamePlayers.GetPlayerObj(playerRef);
            if (otherPlayer == player) continue; // ���� ����

            if (otherPlayer != null && otherPlayer.statHandler.CurHealth > 0)
            {
                PlayerCameraHandler otherCamHandler = otherPlayer.GetComponentInChildren<PlayerCameraHandler>();
                otherCamHandler.spectatorCamera.Priority = 0;
                alivePlayerCameras.Add(otherCamHandler.spectatorCamera);
            }
        }
    }
    public void SwitchToNextFocusingCam(int direction)
    {
        FindAlivePlayers();
        int count = alivePlayerCameras.Count;
        if (count == 0)
        {
            Debug.Log("������ ����");
            return;
        }
        if (curFocusingCam == null)
            return;
        int attempts = 0;
        do
        {
            currentPlayerIndex = (count + currentPlayerIndex + direction) % count;
            attempts++;
        }
        while (alivePlayerCameras[currentPlayerIndex] == curFocusingCam && attempts < count);

        player.cameraHandler.firstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        SetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);

        Debug.Log($"{alivePlayerCameras[currentPlayerIndex]} ���� ��");
    }
    public void SetSpectatorTarget(CinemachineVirtualCamera targetCam)
    {
        if (targetCam == null) return;
        if (curFocusingCam != null)
        {
            curFocusingCam.Priority = 0;    // ���� ��Ŀ���ϴ� ����� �켱���� �����
                                           
            //curFocusingCam.gameObject.GetComponentInParent<PlayerCameraHandler>().isFocusing = false;
            // curFocusingCam�� �����ϰ� �ִ� ��� �����Ϸ���??

            /// �̰ɷ� �ȵȴ�, �ٸ� �ڷ����� �����ؼ� �ű⿡ �����ؾ��Ѵ�
            var prevHandler = curFocusingCam.GetComponentInParent<PlayerCameraHandler>();
            if (prevHandler != null)
            {
                prevHandler.isFocusing = false;
            }
        }

        /// Ÿ���� �ٲٴ� �߿� Ÿ���� �״� ��쿡�� Ÿ���� alivePlayerCameras�� ������ �̵��ؾ��Ѵ�
        curFocusingCam = targetCam;
        curFocusingCam.Priority = 100;    // ���ο� ���� ����� �켱���� ���δ�
        //curFocusingCam.gameObject.GetComponentInParent<PlayerCameraHandler>().isFocusing = true;  // ������ �ϴ� ����� isFocusing�� true�� �ϰ� �ȴ�
        targetCam.gameObject.GetComponentInParent<PlayerCameraHandler>().isFocusing = true; // Ÿ���� isFocusing�� true��
    }
    public void ResetSpectatorTarget()
    {
        // ���� �������� ��� �켱������ �����
        if (curFocusingCam != null)
        {
            curFocusingCam.Priority = 0;
            curFocusingCam.gameObject.GetComponentInParent<PlayerCameraHandler>().isFocusing = false;
        }

        player.cameraHandler.firstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 100;

        // ��Ŀ�� ��� �ʱ�ȭ
        // ��� �÷��̾ OnRespawn�� ���� �� �� �����Ƿ�, �Է� ���� üũ
        if (HasInputAuthority)
        {
            curFocusingCam = player.cameraHandler.firstPersonCamera;
            curFocusingCam.gameObject.GetComponentInParent<PlayerCameraHandler>().isFocusing = true;
        }
    }
    #endregion
}
