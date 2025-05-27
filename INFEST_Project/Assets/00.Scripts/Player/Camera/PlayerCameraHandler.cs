using Cinemachine;
using Fusion;
using Fusion.Addons.SimpleKCC;
using INFEST.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>
/// 카메라의 이동 회전을 다룬다
/// 회전은 상태 변화에 영향을 미치지 않으므로 StateMachine으로부터 독립적으로 작성
/// </summary>
public class PlayerCameraHandler : NetworkBehaviour
{
    public Player player;

    [SerializeField] private Camera _scopeCam;          // scope 전용 카메라
    [SerializeField] private Image _scopeImage;
    [SerializeField] private Transform _cameraHolder;    // 카메라 부모 (X축 회전만 담당)
    public float _sensitivity = 1f;   // 이동에 적용할 민감도
    [SerializeField] private Transform _parentTransform;

    // 마우스의 회전값
    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;

    private Camera _mainCam;    // 1인칭 카메라
    public bool isMenu;
    [SerializeField] private Player _player;

    // 다른 플레이어에 포커스 맞춘다(관전모드)
    public CinemachineVirtualCamera firstPersonCamera;      // 자신의 인플레이 카메라
    public CinemachineVirtualCamera spectatorCamera;    // curFocusingCam가 참조하는 대상의 카메라
    public List<CinemachineVirtualCamera> alivePlayerCameras = new List<CinemachineVirtualCamera>();
    public int currentPlayerIndex = 0;
    List<PlayerRef> playerRefs = new List<PlayerRef>();
    private int previousTime = -1;
    public CinemachineVirtualCamera curFocusingCam;   // 현재 포커싱 대상
    public bool isFocusing = false; // 기본적으로는 false


    private void Awake()
    {
        InitCamera();
        isMenu = false;
    }

    #region 카메라 관련 로직

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

    #region 다른 플레이어 포커싱(관전모드)
    public void FindAlivePlayers()
    {
        // 현재 접속중인 플레이어 정보들 저장
        playerRefs = NetworkGameManager.Instance.gamePlayers.GetPlayerRefs();
        // 이전 생존자 리스트 제거
        alivePlayerCameras.Clear();
        // playerRefs에 있는 플레이어들의 virtualCamera들의 위치를 저장
        foreach (var playerRef in playerRefs)
        {
            // NetworkObject 가져온 다음 Player 컴포넌트에 접근
            Player otherPlayer = NetworkGameManager.Instance.gamePlayers.GetPlayerObj(playerRef);
            if (otherPlayer == player) continue; // 본인 제외

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
            Debug.Log("생존자 없음");
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

        Debug.Log($"{alivePlayerCameras[currentPlayerIndex]} 관전 중");
    }
    public void SetSpectatorTarget(CinemachineVirtualCamera targetCam)
    {
        if (targetCam == null) return;
        if (curFocusingCam != null)
        {
            curFocusingCam.Priority = 0;    // 지금 포커싱하는 대상의 우선순위 낮춘다
                                           
            //curFocusingCam.gameObject.GetComponentInParent<PlayerCameraHandler>().isFocusing = false;
            // curFocusingCam가 참조하고 있는 대상에 접근하려면??

            /// 이걸로 안된다, 다른 자료형을 선언해서 거기에 저장해야한다
            var prevHandler = curFocusingCam.GetComponentInParent<PlayerCameraHandler>();
            if (prevHandler != null)
            {
                prevHandler.isFocusing = false;
            }
        }

        /// 타겟을 바꾸는 중에 타겟이 죽는 경우에도 타겟이 alivePlayerCameras에 있으면 이동해야한다
        curFocusingCam = targetCam;
        curFocusingCam.Priority = 100;    // 새로운 관전 대상의 우선순위 높인다
        //curFocusingCam.gameObject.GetComponentInParent<PlayerCameraHandler>().isFocusing = true;  // 관전을 하는 사람의 isFocusing을 true로 하게 된다
        targetCam.gameObject.GetComponentInParent<PlayerCameraHandler>().isFocusing = true; // 타겟의 isFocusing을 true로
    }
    public void ResetSpectatorTarget()
    {
        // 현재 관전중인 대상 우선순위를 낮춘다
        if (curFocusingCam != null)
        {
            curFocusingCam.Priority = 0;
            curFocusingCam.gameObject.GetComponentInParent<PlayerCameraHandler>().isFocusing = false;
        }

        player.cameraHandler.firstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 100;

        // 포커싱 대상 초기화
        // 모든 플레이어가 OnRespawn를 통해 올 수 있으므로, 입력 권한 체크
        if (HasInputAuthority)
        {
            curFocusingCam = player.cameraHandler.firstPersonCamera;
            curFocusingCam.gameObject.GetComponentInParent<PlayerCameraHandler>().isFocusing = true;
        }
    }
    #endregion
}
