using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SessionListEntry : MonoBehaviour
{
    public TextMeshProUGUI roomName, playerCount;
    public Button joinButton;

    public void JoinRoom()
    {
        NetworkRunner _runner = Matching.Instance.runner;

        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = roomName.text,
            SceneManager = _runner.GetComponent<NetworkSceneManagerDefault>(), // Áß¿ä!
            Scene = scene
        });
    }
}
