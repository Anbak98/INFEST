using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISetView : UIScreen
{
    [Header("���")]
    public Slider brightSlider;
    public Image brightImage;
    public TextMeshProUGUI brightText;

    [Header("�ɼ� ��Ӵٿ�")]
    public TMP_Dropdown screenrate;
    public TMP_Dropdown resolution;
    public TMP_Dropdown graphic;
    public TMP_Dropdown display;

    private Resolution[] _resolutions;

    public override void Awake()
    {
        base.Awake();
        brightSlider.onValueChanged.AddListener(Brightness);

        SetUpScreenRate();
        SetUpResolution();
        SetUpGraphic();
        SetUpDisplay();
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

        brightText.text = $"{(value * 100f).ToString("F0")}%";
    }

    private void SetUpScreenRate()
    {
        screenrate.ClearOptions();
        var options = new List<string> { "4:3", "16:9", "21:9" };
        screenrate.AddOptions(options);
        screenrate.onValueChanged.AddListener(index =>
        {
            Debug.Log($"Set ScreenRate: {options[index]}");
        });
    }

    private void SetUpResolution()
    {
        resolution.ClearOptions();

        var targetResolutions = new List<Resolution>
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1280, height = 720 }
    };


        var options = targetResolutions.Select(r => $"{r.width}x{r.height}").Distinct().ToList();
        resolution.AddOptions(options);

        resolution.onValueChanged.AddListener(index =>
        {
            var res = targetResolutions[index];
            Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
            Debug.Log($"Set resolution: {res.width}x{res.height}");
        });
    }

    private void SetUpGraphic()
    {
        graphic.ClearOptions();

        //����Ƽ�� ��ϵ� ��ü ǰ�� �̸� ��������
        var qualityNames = QualitySettings.names.ToList();

        //��Ӵٿ �����ְ� ���� �̸���
        var graphicNames = new List<string> { "Very Low", "Medium", "Ultra" };

        //�� ���ϴ� �̸��� ���� �� ��° �������� ã�Ƶα�
        var levelIndices = graphicNames.Select(name => qualityNames.FindIndex(q => q == name)).ToList();

        //��Ӵٿ� �ɼ����� ���ϴ� �̸��� �߰�
        graphic.AddOptions(graphicNames);

        //���� ǰ�� ������ desiredNames �� ������� ����
        int currentLevel = QualitySettings.GetQualityLevel();
        int currentDropdownIndex = levelIndices.FindIndex(idx => idx == currentLevel);
        if (currentDropdownIndex >= 0)
            graphic.value = currentDropdownIndex;

        //��Ӵٿ� ������ �ٲ�� ���� ǰ�� ���� ����
        graphic.onValueChanged.AddListener(dropdownIndex =>
        {
            int qualityLevel = levelIndices[dropdownIndex];
            if (qualityLevel >= 0)
            {
                QualitySettings.SetQualityLevel(qualityLevel);
                Debug.Log($"Set Graphic: {graphicNames[dropdownIndex]} (Level {qualityLevel})");
            }
            else
            {
                Debug.LogWarning($"Quality level for '{graphicNames[dropdownIndex]}' not found.");
            }
        });
    }

    private void SetUpDisplay()
    {
        display.ClearOptions();
        var options = new List<string> { "Full Screen", "Window Screen" };
        display.AddOptions(options);

        display.onValueChanged.AddListener(index =>
        {
            var mode = (index == 0) ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.fullScreenMode = mode;
            Debug.Log($"Set Display: {(index == 0 ? "Full Screen" : "Window Screen")}");
        });
    }
}
