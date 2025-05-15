using UnityEngine;

public class TutorialController : MonoBehaviour
{
    UITutorial _tutorial;
    public int page = 0;
    public GameObject[] wall;
    public MVPStageSpawner spawner;
    [SerializeField] private GameObject _spwaPosition;
    MonsterSpawner monsterSpawner;
    public void TextChanged()
    {
        _tutorial = Global.Instance.UIManager.Show<UITutorial>();

        switch (page)
        {
            case 0:
                WallDeactivate(0);
                _tutorial.tutorialText.text = "�������� �̵��ϼ���!";
                _tutorial.toolTipText.text = "W,S,A,D - �̵�\r\nSpace Bar - ����\r\nShift - �޸���";
                break;
            case 1:
                monsterSpawner = spawner.monsterSpawner;
                WallAtivate(0);
                monsterSpawner.AllocateSpawnCommand(1001,1, _spwaPosition.transform.position);
                _tutorial.tutorialText.text = "������ �ִ� ���� ��������!";
                _tutorial.toolTipText.text = "Left Mouse - ���\r\nRight Mouse - ����\r\nMouse Wheel- ���� ����";
                break;
            case 2:
                WallDeactivate(1);
                WallDeactivate(0);
                _tutorial.tutorialText.text = "������ �̿��ؼ� �����ϼ���!\r\n(������ �������� Ȱ��ȭ�˴ϴ�.)";
                _tutorial.toolTipText.text = "F - ��ȣ�ۿ�";
                break;
            case 3:
                _tutorial.tutorialText.text = "����ͷ� �̵��ϼ���!";
                _tutorial.toolTipText.text = "";
                break;
            case 4:
                monsterSpawner.AllocateSpawnCommand(1001, 100, Vector3.zero);
                _tutorial.tutorialText.text = "�������� Ȱ���Ͽ� ���� ��� �����ϼ���.";
                _tutorial.toolTipText.text = "G - ����ź ���\r\nE - ȸ�� ������ ���\r\nV - ��ġ ������ ���";
                break;
            default:
                return;
        }

        page++;

}

    public void ResetTutorial()
    {
        page = 0;
        TextChanged();
    }

    public void WallAtivate(int index)
    {
        wall[index].gameObject.SetActive(true);
    }

    public void WallDeactivate(int index)
    {
        wall[index].gameObject.SetActive(false);
    }
}

