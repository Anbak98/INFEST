using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;

public class WebGameEndView : UIScreen
{
    public GameObject VictoryHeader;
    public GameObject DefeatHeader;
    public TMPro.TMP_Text Tooltip;

    public void Victory()
    {
        VictoryHeader.SetActive(true);
        DefeatHeader.SetActive(false);
        Tooltip.text = "How did you do that?!";
    }

    public void Defeat()
    {
        VictoryHeader.SetActive(false);
        DefeatHeader.SetActive(true);
        Tooltip.text = "Good performance! Hope you had fun.";
    }

    public async void OnPressedExitButton()
    {
        await FindAnyObjectByType<NetworkRunner>().Shutdown();
        SceneManager.LoadScene(0);
    }
}
