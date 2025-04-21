using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestGameUI : MonoBehaviour
{
    public TestPlay testPlay;
    public NetworkRunner runner;
    [SerializeField] private UIScoreboardView _scoreboardView;
    [SerializeField] private UIMenuView _menuView;
    [SerializeField] private UISetView _setView;
    [SerializeField] private UIStateView _stateView;


    private void Update()
    {
        runner = testPlay.Runner;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _scoreboardView.Show();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            _scoreboardView.Hide();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_menuView.gameObject.activeSelf)
            {
                if (_setView.gameObject.activeSelf)
                {
                    _setView.Hide();
                }
                else
                {
                    _menuView.Hide();
                }
            }
            else
            {
                _menuView.Show();
            }
        }        
    }
}
