using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialController tutorialController;
    private int _playerLayer = 7;
    private bool _triggerChk = false;

    // Start is called before the first frame update
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == _playerLayer && !_triggerChk)
        {
            tutorialController.TextChanged();
            _triggerChk=true;
        }
    }
}
