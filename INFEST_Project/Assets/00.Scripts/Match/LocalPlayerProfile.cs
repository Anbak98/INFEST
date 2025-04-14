using Fusion;
using System.Threading.Tasks;
using UnityEngine;

public class LocalPlayerProfile : MonoBehaviour
{
    public NetworkRunner Runner;
    [SerializeField] private NetworkPrefabRef _roomPrefab;

    [Header("User Info")]
    public string userId;
    public string nickName;
    public JOB JOB;

    [Header("Input Field")]
    public TMPro.TMP_InputField nickNameInput;
    public TMPro.TMP_InputField JobInput;

    public void Start()
    {
        userId = Runner.UserId;
        nickName = PlayerPrefs.GetString("Nickname");
        JOB = (JOB)PlayerPrefs.GetInt("Job");
    }

    public void Set()
    {
        PlayerPrefs.SetString("Nickname", nickNameInput.text);
    }

    public void JoinPrivateRoom()
    {
        Runner = gameObject.AddGetComponent<NetworkRunner>();
        Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "Private",
            SceneManager = gameObject.AddGetComponent<NetworkSceneManagerDefault>(),
            IsOpen = false,
        });
    }

    public async Task QuickMatch()
    {
        Runner = gameObject.AddGetComponent<NetworkRunner>();
        await Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SceneManager = gameObject.AddGetComponent<NetworkSceneManagerDefault>(),
        });
    }

    public async void EnterRoom(string code)
    {
        await Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = code,
            SceneManager = gameObject.AddGetComponent<NetworkSceneManagerDefault>()
        });
    }

    public void ExitRoom()
    {
        JoinPrivateRoom();
    }

    public async void OnPressedEnterRoomButton()
    {
        await QuickMatch(); // Wait for the match to start

        if (Runner.IsSharedModeMasterClient)
        {
            Runner.Spawn(_roomPrefab);
        }
    }
}
