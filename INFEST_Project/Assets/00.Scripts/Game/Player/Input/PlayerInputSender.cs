using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerInputHandler에서 받은 입력을 네트워크로 전송한다
/// 로컬 유저가 전송할때는 PlayerRef 사용하지 않아도 된다는데 조금 더 알아보자
/// 
/// 다만.. OnInput을 호출하기 위해서는 
/// runner.ProvideInput = true; (이건 전역설정이라서 StandardSpawner의 StartGame에 선언되어있으므로 안해도 된다)
/// runner.AddCallbacks(playerInputSender);
/// 이 있어야 한다
/// 그래야 입력을 Fusion에 넘겨줄 클래스인 PlayerInputSender가 콜백으로 등록되어 
/// PlayerInputSender에 있는 OnInput이 콜백으로 등록되어 키 입력시 호출된다
/// </summary>
public class PlayerInputSender : MonoBehaviour, INetworkRunnerCallbacks
{
    public PlayerInputHandler playerInputHandler;
    public NetworkRunner runner; // 혹은 외부에서 할당 받도록

    void Awake()
    {
        if (runner == null)
        {
            // Spawner 또는 GameManager를 찾아서 runner 할당
            GameObject spawnerObj = GameObject.Find("StandardSpawner"); // 혹은 tag나 다른 방법으로
            if (spawnerObj != null)
            {
                StandradSpawner spawner = spawnerObj.GetComponent<StandradSpawner>();
                if (spawner != null)
                {
                    runner = spawner._runner;
                    runner.AddCallbacks(this);
                }
                else
                {
                    Debug.LogError("Spawner 컴포넌트가 없음");
                }
            }
            else
            {
                Debug.LogError("Spawner 오브젝트를 찾을 수 없음");
            }
        }
        else
        {
            runner.AddCallbacks(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerInputHandler == null)
        {
            playerInputHandler = FindAnyObjectByType<PlayerInputHandler>();
            if (playerInputHandler == null)
            {
                Debug.LogError("PlayerInputHandler가 씬에 없습니다!");
            }
        }
        //PlayerRef playerRef = new PlayerRef();

        // OnInput을 호출하는 클래스에서 AddCallbacks가 호출되어야한다
        runner.AddCallbacks(this);
    }

    /// <summary>
    /// runner.AddCallbakcs(this)가 호출되어야 OnInput이 호출된다
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="input"></param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (playerInputHandler == null || !playerInputHandler.HasInputAuthority)
            return;

        //Debug.Log("OnInput 진입");
        NetworkInputData? networkInput = playerInputHandler.GetNetworkInput();

        if (networkInput.HasValue)
        {
            input.Set(networkInput.Value);
        }
    }
    #region 나머지는 구현하지 않는다
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    #endregion
}
