using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetView : UIScreen
{
    public Slider brightSlider;
    public Image brightImage;

    public override void Awake()
    {
        base.Awake();
        brightSlider.onValueChanged.AddListener(Brightness);
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void Brightness(float value)
    {
        float alpha = Mathf.Min((1f - value) * 0.9f, 0.9f);
        Color c = brightImage.color;
        c.a = alpha;
        brightImage.color = c;
    }
}
