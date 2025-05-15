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
                _tutorial.tutorialText.text = "공원으로 이동하세요!";
                _tutorial.toolTipText.text = "W,S,A,D - 이동\r\nSpace Bar - 점프\r\nShift - 달리기";
                break;
            case 1:
                monsterSpawner = spawner.monsterSpawner;
                WallAtivate(0);
                monsterSpawner.AllocateSpawnCommand(1001,1, _spwaPosition.transform.position);
                _tutorial.tutorialText.text = "공원에 있는 좀비를 잡으세요!";
                _tutorial.toolTipText.text = "Left Mouse - 사격\r\nRight Mouse - 조준\r\nMouse Wheel- 무기 변경";
                break;
            case 2:
                WallDeactivate(1);
                WallDeactivate(0);
                _tutorial.tutorialText.text = "상점을 이용해서 정비하세요!\r\n(상점은 랜덤으로 활성화됩니다.)";
                _tutorial.toolTipText.text = "F - 상호작용";
                break;
            case 3:
                _tutorial.tutorialText.text = "빈공터로 이동하세요!";
                _tutorial.toolTipText.text = "";
                break;
            case 4:
                monsterSpawner.AllocateSpawnCommand(1001, 100, Vector3.zero);
                _tutorial.tutorialText.text = "아이템을 활용하여 좀비를 잡고 생존하세요.";
                _tutorial.toolTipText.text = "G - 수류탄 사용\r\nE - 회복 아이템 사용\r\nV - 설치 아이템 사용";
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

