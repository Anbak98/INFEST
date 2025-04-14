using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMessagePopup : UIScreen, IPopup
{
    [SerializeField] protected TMP_Text _text;
    [SerializeField] protected TMP_Text _header;
    [SerializeField] protected Button _button;

    public virtual void OpenPopup<T>(T data)
    {
        if (data is not TooltipData tooltipData)
            return;

        _header.text = tooltipData.header;
        _text.text = tooltipData.msg;

        Show();
    }

}

public class TooltipData
{
    public string msg;
    public string header;

    public TooltipData(string msg, string header)
    {
        this.msg = msg;
        this.header = header;
    }
}