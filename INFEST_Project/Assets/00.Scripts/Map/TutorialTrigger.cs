using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialController tutorialController;
    private int _playerLayer = 7;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            tutorialController.TextChanged();
            Destroy(gameObject);
        }
    }
}
