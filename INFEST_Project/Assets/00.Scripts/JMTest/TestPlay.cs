using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlay : MonoBehaviour
{
    [SerializeField] private UIScoreboardView scoreboardView;

    private void Update()
    {
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
