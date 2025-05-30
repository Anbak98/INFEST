using UnityEngine;

public class MatchInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Global.Instance.UIManager.UIList.ContainsKey("UIExitPopup"))
            {
                // UIExitPopup을 한 번 이상 키고 ESC눌렀을 때
                if (Global.Instance.UIManager.UIList["UIExitPopup"].gameObject.activeSelf)
                    Global.Instance.UIManager.Hide<UIExitPopup>();
                else if (Global.Instance.UIManager.UIList.ContainsKey("UIMainSetView"))
                {
                    if (Global.Instance.UIManager.UIList["UIMainSetView"].gameObject.activeSelf)
                        Global.Instance.UIManager.Hide<UIMainSetView>();
                    else
                        Global.Instance.UIManager.Show<UIMainSetView>();
                }
            }
            else if (Global.Instance.UIManager.UIList.ContainsKey("UIMainSetView"))
            {
                // UIMainSetView만 한 번 이상 키고 ESC 눌렀을 때
                if (Global.Instance.UIManager.UIList["UIMainSetView"].gameObject.activeSelf)
                    Global.Instance.UIManager.Hide<UIMainSetView>();
                else
                    Global.Instance.UIManager.Show<UIMainSetView>();
            }
            else
            {
                // 처음 ESC 눌렀을 때
                Global.Instance.UIManager.Show<UIMainSetView>();
            }
        }
    }
}
