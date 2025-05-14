using UnityEngine;

public class TutorialController : MonoBehaviour
{
    UITutorial _tutorial;
    int _page = 0;

    public void TextChanged()
    {
        _tutorial = Global.Instance.UIManager.Show<UITutorial>();

        switch (_page)
        {
            case 0:
                _tutorial.tutorialText.text = "W,S,A,D - �̵�\r\nSpace Bar - ����\r\nShift - �޸���";
                break;
            case 1:
                _tutorial.tutorialText.text = "Left Mouse - ���\r\nRight Mouse\r\n - ����\r\nMouse Wheel- ���� ����";
                break;
            default:
                return;
        }

        _page++;

}

    public void ResetTutorial()
    {
        _page = 0;
        TextChanged();
    }
}

