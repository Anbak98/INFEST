using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISetProfile : UIScreen
{
    [SerializeField] private TMP_InputField _nickNameText;

    protected override void Start()
    {
        base.Start();
        AnalyticsManager.SendFunnelStep(1);
    }

    public override void OnShow()
    {
        base.OnShow();

        Debug.Log($"Call {SceneManager.GetActiveScene()}");
    }

    public void OnPressedSetNickname()
    {
        AnalyticsManager.SendFunnelStep(2);
        PlayerPrefs.SetString("Nickname", _nickNameText.text);
        FindAnyObjectByType<ScreenRoom>().UpdateUI(null);
        Global.Instance.UIManager.Show<UITutorialAnswer>();
        Global.Instance.UIManager.Hide<UISetProfile>();
    }
}
