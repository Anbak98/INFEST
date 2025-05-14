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
                _tutorial.tutorialText.text = "W,S,A,D - 이동\r\nSpace Bar - 점프\r\nShift - 달리기";
                break;
            case 1:
                _tutorial.tutorialText.text = "Left Mouse - 사격\r\nRight Mouse\r\n - 조준\r\nMouse Wheel- 무기 변경";
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

