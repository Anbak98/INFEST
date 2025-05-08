using UnityEngine;
using UnityEngine.UI;

public class UIBrightView : UIScreen
{
    public Image brightOverlayImage;

    public void SetAlpha(float alpha)
    {
        Color c = brightOverlayImage.color;
        c.a = alpha;
        brightOverlayImage.color = c;
    }
}
