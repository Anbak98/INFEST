using TMPro;
using UnityEngine.UI;

public class UIStore : UIBase
{
    public TextMeshProUGUI interactionText;
    public Image panel;
    private void Start()
    {
        panel.gameObject.SetActive(false);
        interactionText.gameObject.SetActive(false);
    }
}
