using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    public NetworkRunner runner;
    public NetworkObject prefab;

    private void Start()
    {
        runner = gameObject.AddGetComponent<NetworkRunner>();
        StartGame();
    }
    public async void StartGame()
    {
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
        });
        Debug.Log("hh");
        runner.Spawn(prefab);
        runner.Spawn(prefab);
        runner.Spawn(prefab);
        runner.Spawn(prefab);
        runner.Spawn(prefab);
        Debug.Log("hh");
    }
}
