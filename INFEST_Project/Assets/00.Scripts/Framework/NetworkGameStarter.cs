using Fusion.Statistics;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkGameStarter : MonoBehaviour
{
    private NetworkRunner _runner;

    public bool IsDebug = false;

    private void Start()
    {
        if (!IsDebug)
        {
            _runner = FindAnyObjectByType<NetworkRunner>();

            if (_runner != null)
            {
                if (_runner.IsSharedModeMasterClient)
                {
                    TryStartGame(GameMode.Host, PlayerPrefs.GetString("RoomCode"));
                }
                else if (_runner.IsClient)
                {
                    TryStartGame(GameMode.Client, PlayerPrefs.GetString("RoomCode"));
                }
                else if (_runner.IsSinglePlayer)
                {
                    TryStartGame(GameMode.Single, PlayerPrefs.GetString("RoomCode"));
                }
            }
        }
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
        UILoading ui = Global.Instance.UIManager.Show<UILoading>();
        INetworkRunnerCallbacks _callbacks = GetComponent<INetworkRunnerCallbacks>();
        StartGameResult result;
        int retryCount = 0;

        _runner = FindAnyObjectByType<NetworkRunner>();

        if (mode == GameMode.Single)
        {
            ui.loadingText.text = $"싱글 게임을 준비 중입니다... ";
        }
        else if (mode == GameMode.Host)
        {
            ui.loadingText.text = $"게임을 생성 중입니다... [{sessionName}] \n\n\n ";
        }
        else if (mode == GameMode.Client)
        {
            ui.loadingText.text = $"호스트가 게임을 생성 중입니다... [{sessionName}] \n\n\n ";
        }

        do
        {
            if (_runner != null)
            {
                await _runner.Shutdown();
            }

            GameObject _runnerObject = new GameObject($"Player({mode})");
            _runner = _runnerObject.AddGetComponent<NetworkRunner>();

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
                SceneManager = gameObject.AddGetComponent<NetworkSceneManagerDefault>()
            });
            _runner.AddCallbacks(_callbacks);

            if (mode == GameMode.Single)
            {
                ui.loadingText.text = $"싱글 게임을 준비 중입니다... {retryCount} \n\n\n {result}";
            }
            else if (mode == GameMode.Host)
            {
                ui.loadingText.text = $"게임을 생성 중입니다... {retryCount} [{sessionName}] \n\n\n {result}";
            }
            else if (mode == GameMode.Client)
            {
                ui.loadingText.text = $"호스트가 게임을 생성 중입니다... {retryCount}  [{sessionName}] \n\n\n {result}";
            }

            if(result.ShutdownReason == ShutdownReason.GameIdAlreadyExists)
            {
                mode = GameMode.Client;
            }

            ++retryCount;
        } while (!result.Ok && retryCount < 15);
    }
}
