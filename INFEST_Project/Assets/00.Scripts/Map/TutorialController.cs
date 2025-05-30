using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public UITutorial tutorial;
    public int page = 0;
    public GameObject[] trigger;
    public GameObject[] wall;
    public EnhancedMonsterSpawner _monsterSpawner;
    public GameObject arrow;
    public GameObject[] shopArrow;
    public Transform[] arrowPosition; 
    [SerializeField] private GameObject _spwaPosition;
    [HideInInspector] public Player player;
    Vector3 _pos;
    public void TextChanged()
    {
        tutorial = Global.Instance.UIManager.Show<UITutorial>();
        if(arrowPosition.Length > page)
        {
            _pos = arrowPosition[page].position;
            _pos.y = 7f;
            arrow.transform.position = _pos;
        }
        
        switch (page)
        {
            case 0:
                WallDeactivate(0);
                break;
            case 1:
                WallAtivate(0);
                _monsterSpawner.SpawnOnPos(_spwaPosition.transform,1001);
                break;
            case 2:
                WallDeactivate(1);
                WallDeactivate(0);
                shopArrow[0].gameObject.SetActive(true);
                shopArrow[1].gameObject.SetActive(true);
                player.statHandler.CurGold += 2000;
                break;
            case 3:
                WallDeactivate(2);
                shopArrow[0].gameObject.SetActive(false);
                shopArrow[1].gameObject.SetActive(false);
                break;
            case 4:
                arrow.gameObject.SetActive(false);
                break;
            default:
                return;
        }
        tutorial.TextChanged(page);
        page++;

}

    public void ResetTutorial()
    {
        page = 0;
        WallDeactivate(0);
        WallAtivate(1);
        WallAtivate(2);
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

