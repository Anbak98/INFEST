using Fusion.Statistics;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkGameStarter : MonoBehaviour
{
    private NetworkRunner _runner;

    private void Start()
    {
#if !UNITY_EDITOR
        GameMode mode = (GameMode)PlayerPrefs.GetInt("GameMode");

        TryStartGame(mode, PlayerPrefs.GetString("RoomCode"));
#endif
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                TryStartGame(GameMode.Host, "DEBUG");
            }
            else if (GUI.Button(new Rect(0, 40, 200, 40), "Client"))
            {
                TryStartGame(GameMode.Client, "DEBUG");
            }
            else if (GUI.Button(new Rect(0, 80, 200, 40), "Single"))
            {
                TryStartGame(GameMode.Single, "DEBUG");
            }
        }
        //if (_runner != null)
        //{
        //    if (_runner.IsServer)
        //    {
        //        if (GUI.Button(new Rect(0, 40, 200, 40), "CreateNew"))
        //            TryStartGame(GameMode.Host, "DEBUG";
        //    }
        //    if (_runner.IsClient)
        //    {
        //        if (GUI.Button(new Rect(0, 40, 200, 40), "Reconnect"))
        //            TryStartGame(GameMode.Client, "DEBUG");
        //    }
        //}
    }

    private async void TryStartGame(GameMode mode, string sessionName)
    {
        INetworkRunnerCallbacks _callbacks = GetComponent<INetworkRunnerCallbacks>();
        StartGameResult result;
        int retryCount = 0;

        _runner = FindAnyObjectByType<NetworkRunner>();

        do
        {
            if (_runner != null)
            {
                await _runner.Shutdown();
            }

            GameObject _runnerObject = new GameObject($"Player({mode})");
            _runner = _runnerObject.AddComponent<NetworkRunner>();

            // Create the NetworkSceneInfo from the current scene
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            _runner.ProvideInput = true;

            // Start or join (depends on gamemode) a session with a specific name
            result = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = sessionName,
                Scene = scene,
                //ObjectProvider = gameObject.AddComponent<PoolObjectProvider>(),
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
            _runner.AddCallbacks(_callbacks);

            ++retryCount;
        } while (!result.Ok && retryCount < 15);
    }
}
