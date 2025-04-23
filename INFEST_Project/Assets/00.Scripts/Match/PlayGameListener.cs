using Fusion;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGameListener : MonoBehaviour
{
    public NetworkRunner _runner;

    private const string PlaySceneName = "PlayStage(MVP)";
    private const string SessionName = "HostGame";

    public bool IsStarted = false;
    public bool IsHost = false;

    public void Start()
    {
        _runner = gameObject.AddGetComponent<NetworkRunner>();
    }

    public void Update()
    {
        if (IsStarted)
        {
            StartCoroutine(SwitchToHostMode());
            IsStarted = false;
        }
    }

    public IEnumerator SwitchToHostMode()
    {
        // �� �ε�
        var asyncLoad = SceneManager.LoadSceneAsync(PlaySceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // ���ο� Runner ����
        var runnerGO = new GameObject("Runner (Host)");
        var newRunner = runnerGO.AddComponent<NetworkRunner>();
        newRunner.ProvideInput = true;

        // �� �Ŵ��� �ʿ�
        var sceneManager = runnerGO.AddComponent<NetworkSceneManagerDefault>();

        //if(IsHost)
        //{
            yield return newRunner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = SessionName,
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = sceneManager
            });
        //}
        //else
        //{
        //    yield return ClientTryConnect(newRunner, sceneManager);

        //}

        yield return _runner.Shutdown();
    }

    //public async Task ClientTryConnect(NetworkRunner newRunner, NetworkSceneManagerDefault sceneManager)
    //{

    //    StartGameResult result;
    //    do
    //    {
    //        result = await newRunner.StartGame(new StartGameArgs
    //        {
    //            GameMode = GameMode.Client,
    //            SessionName = SessionName,
    //            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
    //            SceneManager = sceneManager
    //        });

    //    } while (!result.Ok);
    //}
}
