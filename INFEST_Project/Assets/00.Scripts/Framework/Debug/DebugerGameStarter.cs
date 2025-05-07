using Fusion;
using Fusion.Statistics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugerGameStarter : MonoBehaviour
{
    [Header("Connection Message Related")]
    [SerializeField] private GameObject _sessionConnectionMessageUI;
    [SerializeField] private TMPro.TMP_Text _sessionConnectionMessage;

    [Header("Debug Message Related")]
    [SerializeField] private GameObject _sessionDebugMessageUI;
    [SerializeField] private TMPro.TMP_Text _sessionDebugMessage;

    [Header("Error Message Related")]
    [SerializeField] private GameObject _sessionErrorMessageUI;
    [SerializeField] private TMPro.TMP_Text _sessionErrorMessage;

    private NetworkRunner _runner;
    private INetworkRunnerCallbacks _callbacks;

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                TryStartGame(GameMode.Host);
            }
            else if (GUI.Button(new Rect(0, 40, 200, 40), "Client"))
            {
                TryStartGame(GameMode.Client);
            }
            else if (GUI.Button(new Rect(0, 80, 200, 40), "Single"))
            {
                TryStartGame(GameMode.Single);
            }
        }
        if (_runner != null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Clear Debug"))
                SetDebugMessage(string.Empty);
            if (_runner.IsClient)
            {
                if (GUI.Button(new Rect(0, 40, 200, 40), "Reconnect"))
                    TryStartGame(GameMode.Client);
            }
        }
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible =true;
    }

    private async void TryStartGame(GameMode mode)
    {
        SetConnectionMessage(string.Empty);
        ShowConnectionMessage();
        ShowDebugMessage();
        _callbacks = GetComponent<INetworkRunnerCallbacks>();
        StartGameResult result;
        int retryCount = 0;

        do
        {
            SetConnectionMessage($"Retry...{retryCount}");
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
                SessionName = "TestRoom",
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
            _runner.AddCallbacks(_callbacks);
            SetDebugMessage($"Retry...{retryCount}\n{mode}\n{result}");

            ++retryCount;
        } while (!result.Ok && retryCount < 15);
        HideConnectionMessage();
    }

    public void SetConnectionMessage(string msg)
    {
        _sessionConnectionMessage.text = msg;
    }

    public void SetDebugMessage(string msg)
    {
        _sessionDebugMessage.text = msg;
    }

    public void SetErrorMessage(string msg)
    {
        _sessionErrorMessage.text = msg;
    }

    public void AddConnectionMessage(string msg)
    {
        _sessionConnectionMessage.text += "\n" + msg;
    }

    public void AddDebugMessage(string msg)
    {
        _sessionDebugMessage.text += "\n" + msg;
    }

    public void AddErrorMessage(string msg)
    {
        _sessionErrorMessage.text += "\n" + msg;
    }

    public void ShowConnectionMessage()
    {
        _sessionConnectionMessageUI.SetActive(true);
    }

    public void ShowDebugMessage()
    {
        _sessionDebugMessageUI.SetActive(true);
    }

    public void ShowErrorMessage()
    {
        _sessionErrorMessageUI.SetActive(true);
    }

    public void HideConnectionMessage()
    {
        _sessionConnectionMessageUI.SetActive(false);
    }

    public void HideDebugMessage()
    {
        _sessionDebugMessageUI.SetActive(false);
    }

    public void HideErrorMessage()
    {
        _sessionErrorMessageUI.SetActive(false);
    }
}
