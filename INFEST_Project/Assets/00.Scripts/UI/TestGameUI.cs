using Fusion;
using UnityEngine;

public class TestGameUI : MonoBehaviour
{
    public TestPlay testPlay;
    public NetworkRunner runner;
    [SerializeField] private UIScoreboardView scoreboardView;


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
    }
}
