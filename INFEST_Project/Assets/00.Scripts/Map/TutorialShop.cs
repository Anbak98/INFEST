using UnityEngine;

public class TutorialShop : MonoBehaviour
{
    public TutorialController tutorialController;

    private void Update()
    {
        if (tutorialController.page != 3) return;
        if (Player.local.isInteraction)
        {
            tutorialController.TextChanged();
            tutorialController.tutorial.ShowImage();
            Destroy(gameObject);
        }
    }
}
