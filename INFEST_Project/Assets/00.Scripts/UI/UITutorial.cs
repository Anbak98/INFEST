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

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
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
                tutorialText.text = "�������� �̵��ϼ���!";
                toolTipText.text = "W,S,A,D - �̵�\r\nSpace Bar - ����\r\nShift - �޸���";
                break;
            case 1:
                tutorialText.text = "������ �ִ� ���� ��������!";
                toolTipText.text = "Left Mouse - ���\r\nRight Mouse - ����\r\nMouse Wheel- ���� ����";
                break;
            case 2:
                tutorialText.text = "������ �̿��ؼ� �����ϼ���!\r\n(������ �������� Ȱ��ȭ�˴ϴ�.)";
                toolTipText.text = "F - ��ȣ�ۿ�";
                break;
            case 3:
                tutorialText.text = "����ͷ� �̵��ϼ���!";
                toolTipText.text = "";
                break;
            case 4:
                tutorialText.text = "����� ���� ����մϴ�.\r\n(�������� Ȱ���Ͽ� ���� ��� �����ϼ���.)";
                toolTipText.text = "G - ����ź ���\r\nE - ȸ�� ������ ���\r\nV - ��ġ ������ ���";
                break;
            default:
                return;
        }

        page++;
    }


}
