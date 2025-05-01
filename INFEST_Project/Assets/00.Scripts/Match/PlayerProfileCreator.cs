using TMPro;
using UnityEngine;

public class PlayerProfileCreator : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nickNameText;
    [SerializeField] private GameObject _setNicknameUI;
    [SerializeField] private ScreenRoom _ui;

    public void OnPressedSetNickname()
    {
        PlayerPrefs.SetString("Nickname", _nickNameText.text);
        _ui.UpdateUI(null);
        _setNicknameUI.SetActive(false);
    }
}
