using Fusion.Statistics;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    private NetworkRunner _runner;

    private void Start()
    {
        GameMode mode = (GameMode)PlayerPrefs.GetInt("GameMode");

        TryStartGame(mode);
    }

    private async void TryStartGame(GameMode mode)
    {
        INetworkRunnerCallbacks _callbacks = GetComponent<INetworkRunnerCallbacks>();
        StartGameResult result;
        int retryCount = 0;

        do
        {
            if (_runner != null)
            {
                await _runner.Shutdown();
            }

            GameObject _runnerObject = new GameObject($"Player({mode})");
            _runner = _runnerObject.AddComponent<NetworkRunner>();
            _runnerObject.AddComponent<FusionStatistics>();

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

            ++retryCount;
        } while (!result.Ok && retryCount < 15);
    }
}
