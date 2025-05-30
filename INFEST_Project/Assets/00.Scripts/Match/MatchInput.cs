using UnityEngine;

public class MatchInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Global.Instance.UIManager.UIList.ContainsKey("UIExitPopup"))
            {
                // UIExitPopup�� �� �� �̻� Ű�� ESC������ ��
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
                // UIMainSetView�� �� �� �̻� Ű�� ESC ������ ��
                if (Global.Instance.UIManager.UIList["UIMainSetView"].gameObject.activeSelf)
                    Global.Instance.UIManager.Hide<UIMainSetView>();
                else
                    Global.Instance.UIManager.Show<UIMainSetView>();
            }
            else
            {
                // ó�� ESC ������ ��
                Global.Instance.UIManager.Show<UIMainSetView>();
            }
        }
    }
}
