using Fusion;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MatchManager : SingletonBehaviour<MatchManager>
{
    public Room Room;
    public ScreenRoom RoomUI;
    public NetworkRunner Runner;
    public NetworkObject RoomPrefab;

    public GameType SelectedGameType = GameType.BossHunt;
    public GameMap SelectedGameMap = GameMap.MVP;

    public enum Game
    {
        Old,
        New
    }

    public enum GameType
    {
        BossHunt,
        WaveBattleOneVSOne,
        WaveBattleFourVSFour,
    }

    public enum GameMap
    {
        MVP,
        RuinedCity,
        Underground
    }

    public async void QuickMatch()
    {
        if (Runner != null)
            await Runner.Shutdown();

        // 货肺款 Runner 积己
        var runnerGO = new GameObject("Runner (Shared)");
        Runner = runnerGO.AddComponent<NetworkRunner>();
        runnerGO.AddComponent<PlayGameListener>();

        var customProps = new Dictionary<string, SessionProperty>();

        customProps["map"] = (int)SelectedGameMap;
        customProps["type"] = (int)SelectedGameType;

        var result = await Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionProperties = customProps,
            SceneManager = gameObject.AddGetComponent<NetworkSceneManagerDefault>(),
            EnableClientSessionCreation = false,
        });

        if (!result.Ok)
            CreateNewSession(true);
        else
        {
            RoomUI.UpdateUIWhenJoinRoom();
        }
    }

    public async void CreateNewSession(bool IsPublic, string code = "")
    {
        do {
            if (Runner != null)
                await Runner.Shutdown();

            // 货肺款 Runner 积己
            var runnerGO = new GameObject("Runner (Shared)");
            Runner = runnerGO.AddComponent<NetworkRunner>();
            runnerGO.AddComponent<PlayGameListener>();


            var customProps = new Dictionary<string, SessionProperty>();

            customProps["map"] = (int)SelectedGameMap;
            customProps["type"] = (int)SelectedGameType;

            if (code == "")
                code = GenerateSessionCode();

            await Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = code,
                IsVisible = IsPublic,
                SessionProperties = customProps,
                SceneManager = gameObject.AddGetComponent<NetworkSceneManagerDefault>()
            });
        } while (Runner.SessionInfo.PlayerCount > 1);

        if (Runner.IsSharedModeMasterClient)
        {
            Runner.Spawn(RoomPrefab);
        }
    }

    public void PlayerSoloGame()
    {
        Runner.LoadScene("PlayStage(MVP)");
    }

    public void PlayPartyGame()
    {
        if (Room != null)
            Room.HostPlayGame();
    }

    public bool JoinSession(string code)
    {
        JoinSessionRunner(code);

        if (Runner == null) 
            return false;

        return true;
    }

    private async void JoinSessionRunner(string code)
    {
        if (Runner != null)
            await Runner.Shutdown();

        await Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = code,
            EnableClientSessionCreation = false,
            SceneManager = gameObject.AddGetComponent<NetworkSceneManagerDefault>()
        });

        if (Runner.SessionInfo.PlayerCount == 1)
            await Runner.Shutdown();
    }

    private string GenerateSessionCode(int length = 6)
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ123456789";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            sb.Append(chars[Random.Range(0, chars.Length)]);
        }

        return sb.ToString();
    }
}
