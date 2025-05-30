using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : UIScreen
{
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI toolTipText;
    public Image shopImage;
    protected override void Start()
    {
        base.Start();
    }

    public override void OnShow()
    {
        base.OnShow();
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    public void ShowImage()
    {
        shopImage.gameObject.SetActive(true);
    }

    public void TextChanged(int page)
    {
        switch (page)
        {
            case 0:
                tutorialText.text = "공원으로 이동하세요!";
                toolTipText.text = "W,S,A,D - 이동\r\nSpace Bar - 점프\r\nShift - 달리기\r\nCtrl - 앉기";
                break;
            case 1:
                tutorialText.text = "공원에 있는 좀비를 잡으세요!";
                toolTipText.text = "Left Mouse - 사격\r\nRight Mouse - 조준\r\nMouse Wheel Up/Down- 무기 변경";
                break;
            case 2:
                tutorialText.text = "상점을 이용해서 정비하세요!";
                toolTipText.text = "F - 상호작용";
                break;
            case 3:
                tutorialText.text = "빈공터로 이동하세요!";
                toolTipText.text = "";
                break;
            case 4:
                tutorialText.text = "잠시후 좀비가 출몰합니다.\r\n(아이템을 활용하여 좀비를 잡고 생존하세요.)";
                toolTipText.text = "G - 수류탄 사용\r\nE - 회복 아이템 사용\r\nV - 설치 아이템 사용";
                break;
            default:
                return;
        }

        page++;
    }


}
