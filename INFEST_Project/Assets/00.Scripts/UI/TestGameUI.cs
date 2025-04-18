using Fusion;
using UnityEngine;

public class TestGameUI : MonoBehaviour
{
    public TestPlay testPlay;
    public NetworkRunner runner;
    [SerializeField] private UIScoreboardView scoreboardView;
    [SerializeField] private UIMenuView menuView;
    [SerializeField] private UISetView setView;


    private void Update()
    {
        runner = testPlay.Runner;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboardView.Show();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboardView.Hide();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuView.gameObject.activeSelf)
            {
                if (setView.gameObject.activeSelf)
                {
                    setView.Hide();
                }
                else
                {
                    menuView.Hide();
                }
            }
            else
            {
                menuView.Show();
            }
        }
    }
}
