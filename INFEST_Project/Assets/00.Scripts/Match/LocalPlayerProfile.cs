using Fusion;
using System.Threading.Tasks;
using UnityEngine;

public class LocalPlayerProfile : MonoBehaviour
{
    public NetworkRunner Runner;
    [SerializeField] private NetworkPrefabRef _roomPrefab;

    public async Task QuickMatch()
    {
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

    public async void ExitRoom()
    {
        await Runner.Shutdown();
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
