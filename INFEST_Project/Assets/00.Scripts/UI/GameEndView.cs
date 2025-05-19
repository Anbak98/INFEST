using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndView : UIScreen
{
    public GameObject VictoryHeader;
    public GameObject DefeatHeader;
    public TMPro.TMP_Text Tooltip;

    public void Victory()
    {
        VictoryHeader.SetActive(true);
        DefeatHeader.SetActive(false);
        Tooltip.text = "City is secure now";
    }

    public void Defeat()
    {
        VictoryHeader.SetActive(false);
        DefeatHeader.SetActive(true);
        Tooltip.text = "Boss is so strong";
    }

    public void OnPressedExitButton()
    {
        SceneManager.LoadScene(0);
    }
}
