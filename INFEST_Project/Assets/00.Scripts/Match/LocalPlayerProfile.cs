using Fusion;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LocalPlayerProfile : MonoBehaviour
{
    public NetworkRunner _runner;
    [SerializeField] private NetworkPrefabRef _roomPrefab;

    public Room room;

    [Header("User Info")]
    public string userId;
    public string nickName;
    public JOB JOB;

    [Header("Input Field")]
    public TMPro.TMP_InputField nickNameInput;
    public TMPro.TMP_InputField JobInput;

    public bool IsStarted = false;

    public void Start()
    {
        _runner = gameObject.AddGetComponent<NetworkRunner>();
        userId = _runner.UserId;
        nickName = PlayerPrefs.GetString("Nickname");
        JOB = (JOB)PlayerPrefs.GetInt("Job");
    }

    public void Update()
    {
        if (IsStarted)
        {
            StartCoroutine(SwitchToHostMode());
            IsStarted = false;
        }
    }

    public void Set()
    {
        PlayerPrefs.SetString("Nickname", nickNameInput.text);
    }

    //public void JoinPrivateRoom()
    //{
    //    Runner = gameObject.AddGetComponent<NetworkRunner>();
    //    Runner.StartGame(new StartGameArgs()
    //    {
    //        GameMode = GameMode.Shared,
    //        SessionName = "Private",
    //        SceneManager = gameObject.AddGetComponent<NetworkSceneManagerDefault>(),
    //        IsOpen = false,
    //    });
    //}

    public async Task QuickMatch()
    {
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SceneManager = gameObject.AddGetComponent<NetworkSceneManagerDefault>(),
        });
    }

    public async void EnterRoom(string code)
    {
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = code,
            SceneManager = gameObject.AddGetComponent<NetworkSceneManagerDefault>()
        });
    }

    private const string PlaySceneName = "PlayStage(MVP)";
    private const string SessionName = "HostGame";

    public void PlayGame()
    {
        if (_runner.IsSharedModeMasterClient)
        {
            room.RPC_Foo();
        }
    }

    public IEnumerator SwitchToHostMode()
    {
        // 씬 로드
        var asyncLoad = SceneManager.LoadSceneAsync(PlaySceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // 새로운 Runner 생성
        var runnerGO = new GameObject("Runner (Host)");
        var newRunner = runnerGO.AddComponent<NetworkRunner>();
        newRunner.ProvideInput = true;

        // 씬 매니저 필요
        var sceneManager = runnerGO.AddComponent<NetworkSceneManagerDefault>();

        yield return newRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = SessionName,
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = sceneManager
        });

        yield return _runner.Shutdown();
    }

    public async void OnPressedEnterRoomButton()
    {
        await QuickMatch(); // Wait for the match to start

        if (_runner.IsSharedModeMasterClient)
        {
            var foo = _runner.Spawn(_roomPrefab);
            room = foo.GetComponent<Room>();
        }
    }
}
