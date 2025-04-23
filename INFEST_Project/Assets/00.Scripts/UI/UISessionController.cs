using Fusion;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISessionController : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject _sessionEntryUiPrefab;

    [Header("Reference")]
    [SerializeField] private Transform  _sessionListContentParent;

    [Header("Componsets")]
    [SerializeField] private TMP_InputField _sessionName;
    [SerializeField] private TMP_InputField _userNameInput;
    [SerializeField] private TMP_InputField _jobInput;

    [Header("Players Info")]
    [SerializeField] private UIRoomPlayerInfo[] _playerInfos;

    public void UpdateSession(List<SessionInfo> sessionList)
    {
        DeleteOldSessionUI(sessionList);

        foreach (SessionInfo sessionInfo in sessionList)
        {
            if (sessionListUiDictionary.ContainsKey(sessionInfo.Name))
            {
                UpdateEntryUI(sessionInfo);
            }
            else
            {
                CreateEntryUI(sessionInfo);
            }
        }
    }

    public void UpdateRoom(PlayerInfo playerInfo)
    {
        foreach(var info in _playerInfos)
        {
            if(!info.IsSetted)
            {
                info.OnPlayerEnter(playerInfo.Nickname.ToString(), playerInfo.Job.ToString());
                break;
            }
        }
    }

    public void UpdateUserInfo()
    {
        PlayerPrefs.SetString("Nickname", _userNameInput.text);
        PlayerPrefs.SetString("Job", _jobInput.text);
    }

    public void CreateSession()
    {
        Matching.Instance.runner.StartGame(new StartGameArgs()
        {
            SessionName = _sessionName.text,
            GameMode = GameMode.Host
        });
    }

    public void StartGame()
    {
        if(Matching.Instance.runner.IsServer)
        {
            Matching.Instance.runner.LoadScene("PlayScene");
        }
    }

    #region Private Field
    private Dictionary<string, GameObject> sessionListUiDictionary = new();
    #endregion

    #region Private Method
    private void DeleteOldSessionUI(List<SessionInfo> sessionList)
    {
        bool isContained;
        GameObject uiToDelete;

        foreach (var sessionUi in sessionListUiDictionary)
        {
            isContained = false;
            uiToDelete = null;

            foreach (var sessionInfo in sessionList)
            {
                if (sessionUi.Key == sessionInfo.Name)
                {
                    isContained = true;
                    break;
                }
            }

            if (!isContained)
            {
                uiToDelete = sessionUi.Value;
                sessionListUiDictionary.Remove(sessionUi.Key);
                Destroy(uiToDelete);
            }
        }
    }

    private void CreateEntryUI(SessionInfo session)
    {
        
        GameObject newEntry = GameObject.Instantiate(_sessionEntryUiPrefab);
        newEntry.transform.parent = _sessionListContentParent;
        SessionListEntry entryScript = newEntry.GetComponent<SessionListEntry>();
        sessionListUiDictionary.Add(session.Name, newEntry);

        entryScript.roomName.text = session.Name;
        entryScript.playerCount.text = session.PlayerCount.ToString() + "/" + session.MaxPlayers.ToString();
        entryScript.joinButton.interactable = session.IsOpen;

        newEntry.SetActive(session.IsVisible);
    }

    private void UpdateEntryUI(SessionInfo session)
    {
        sessionListUiDictionary.TryGetValue(session.Name, out GameObject entry);

        SessionListEntry entryScript = entry.GetComponent<SessionListEntry>();

        entryScript.roomName.text = session.Name;
        entryScript.playerCount.text = session.PlayerCount.ToString() + "/" + session.MaxPlayers.ToString();
        entryScript.joinButton.interactable = session.IsOpen;

        entry.SetActive(session.IsVisible);
    }

    #endregion
}
