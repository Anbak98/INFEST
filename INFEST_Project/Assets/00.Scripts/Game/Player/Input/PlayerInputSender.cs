using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerInputHandler���� ���� �Է��� ��Ʈ��ũ�� �����Ѵ�
/// ���� ������ �����Ҷ��� PlayerRef ������� �ʾƵ� �ȴٴµ� ���� �� �˾ƺ���
/// 
/// �ٸ�.. OnInput�� ȣ���ϱ� ���ؼ��� 
/// runner.ProvideInput = true; (�̰� ���������̶� StandardSpawner�� StartGame�� ����Ǿ������Ƿ� ���ص� �ȴ�)
/// runner.AddCallbacks(playerInputSender);
/// �� �־�� �Ѵ�
/// �׷��� �Է��� Fusion�� �Ѱ��� Ŭ������ PlayerInputSender�� �ݹ����� ��ϵǾ� 
/// PlayerInputSender�� �ִ� OnInput�� �ݹ����� ��ϵǾ� Ű �Է½� ȣ��ȴ�
/// </summary>
public class PlayerInputSender : MonoBehaviour, INetworkRunnerCallbacks
{
    public PlayerInputHandler playerInputHandler;
    public NetworkRunner runner; // Ȥ�� �ܺο��� �Ҵ� �޵���

    void Awake()
    {
        if (runner == null)
        {
            // Spawner �Ǵ� GameManager�� ã�Ƽ� runner �Ҵ�
            GameObject spawnerObj = GameObject.Find("StandardSpawner"); // Ȥ�� tag�� �ٸ� �������
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
                    Debug.LogError("Spawner ������Ʈ�� ����");
                }
            }
            else
            {
                Debug.LogError("Spawner ������Ʈ�� ã�� �� ����");
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
                Debug.LogError("PlayerInputHandler�� ���� �����ϴ�!");
            }
        }
        //PlayerRef playerRef = new PlayerRef();

        // OnInput�� ȣ���ϴ� Ŭ�������� AddCallbacks�� ȣ��Ǿ���Ѵ�
        runner.AddCallbacks(this);
    }

    /// <summary>
    /// runner.AddCallbakcs(this)�� ȣ��Ǿ�� OnInput�� ȣ��ȴ�
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="input"></param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (playerInputHandler == null || !playerInputHandler.HasInputAuthority)
            return;

        //Debug.Log("OnInput ����");
        NetworkInputData? networkInput = playerInputHandler.GetNetworkInput();

        if (networkInput.HasValue)
        {
            input.Set(networkInput.Value);
        }
    }
    #region �������� �������� �ʴ´�
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
