using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISetView : UIScreen
{
    [Header("밝기")]
    public Slider brightSlider;
    public UIBrightView brightImage;
    public TextMeshProUGUI brightText;

    [Header("옵션 드롭다운")]
    public TMP_Dropdown screenrate;
    public TMP_Dropdown resolution;
    public TMP_Dropdown graphic;
    public TMP_Dropdown display;

    [Header("민감도")]
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityText;

    [Header("저장버튼")]
    public Button saveBtn;

    private Resolution[] _resolutions;

    private float _originalBrightness;
    private int _originalScreenRateIndex;
    private int _originalResolutionIndex;
    private int _originalGraphicIndex;
    private int _originalDisplayIndex;
    private float _originalSensitivity;

    PlayerCameraHandler _playerCameraHandler;

    public override void Awake()
    {
        base.Awake();

        brightImage = Global.Instance.UIManager.Get<UIBrightView>();

        _playerCameraHandler = FindObjectOfType<PlayerCameraHandler>();        

        _originalBrightness = brightSlider.value;
        _originalScreenRateIndex = screenrate.value;
        _originalResolutionIndex = resolution.value;
        _originalGraphicIndex = graphic.value;
        _originalDisplayIndex = display.value;
        _originalSensitivity = sensitivitySlider.value;

        saveBtn.gameObject.SetActive(false);

        brightSlider.onValueChanged.AddListener(value =>
        {
            Brightness(value);
            CheckForChanges();
        });

        sensitivitySlider.onValueChanged.AddListener(value =>
        {
            SetSensitivity(value);
            CheckForChanges();
        });


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
        brightImage.SetAlpha(alpha);
        brightText.text = $"{(value * 100f).ToString("F0")}%";
    }

    public void SetSensitivity(float value)
    {
        _playerCameraHandler._sensitivity = value;
        sensitivityText.text = $"{(value * 10).ToString("F0")}";
    }

    private void SetUpScreenRate()
    {
        screenrate.ClearOptions();
        var options = new List<string> { "4:3", "16:10", "16:9", "21:9" };
        screenrate.AddOptions(options);
        screenrate.onValueChanged.AddListener(index =>
        {
            Debug.Log($"Set ScreenRate: {options[index]}");
            CheckForChanges();
        });
    }

    private void SetUpResolution()
    {
        resolution.ClearOptions();

        _resolutions = Screen.resolutions;

        var options = _resolutions.Select(r => $"{r.width}x{r.height}").Distinct().ToList();

        resolution.AddOptions(options);

        resolution.onValueChanged.AddListener(index =>
        {
            var res = _resolutions[index];
            Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
            Debug.Log($"Set resolution: {res.width}x{res.height}");
            CheckForChanges();
        });
    }

    private void SetUpGraphic()
    {
        graphic.ClearOptions();

        //유니티에 등록된 전체 품질 이름 가져오기
        var qualityNames = QualitySettings.names.ToList();

        //드롭다운에 보여주고 싶은 이름들
        //var graphicNames = new List<string> { "매우 낮음", "낮음", "보통", "높음", "매우 높음" };
        var graphicNames = new List<string> { "Very Low", "Medium", "Ultra" };

        //각 원하는 이름이 실제 몇 번째 레벨인지 찾아두기
        var levelIndices = graphicNames.Select(name => qualityNames.FindIndex(q => q == name)).ToList();

        //드롭다운 옵션으로 원하는 이름만 추가
        graphic.AddOptions(graphicNames);

        //현재 품질 레벨이 desiredNames 중 어디인지 설정
        int currentLevel = QualitySettings.GetQualityLevel();
        int currentDropdownIndex = levelIndices.FindIndex(idx => idx == currentLevel);
        if (currentDropdownIndex >= 0)
            graphic.value = currentDropdownIndex;

        //드롭다운 선택이 바뀌면 실제 품질 레벨 설정
        graphic.onValueChanged.AddListener(dropdownIndex =>
        {
            int qualityLevel = levelIndices[dropdownIndex];
            if (qualityLevel >= 0)
            {
                QualitySettings.SetQualityLevel(qualityLevel);
                Debug.Log($"Set Graphic: {graphicNames[dropdownIndex]} (Level {qualityLevel})");
                CheckForChanges();
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

            var selectedResolution =
            _resolutions.Select(r => new Vector2Int(r.width, r.height)).Distinct().ElementAt(resolution.value);
            Screen.SetResolution(selectedResolution.x, selectedResolution.y, mode);
            Debug.Log($"Set Display: {(index == 0 ? "Full Screen" : "Window Screen")}");
            CheckForChanges();
        });
    }

    private void CheckForChanges()
    {
        bool changed =
            !Mathf.Approximately(brightSlider.value, _originalBrightness) ||
            screenrate.value != _originalScreenRateIndex ||
            resolution.value != _originalResolutionIndex ||
            graphic.value != _originalGraphicIndex ||
            display.value != _originalDisplayIndex ||
            !Mathf.Approximately(sensitivitySlider.value, _originalSensitivity);


        saveBtn.gameObject.SetActive(changed);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Brightness", brightSlider.value);
        PlayerPrefs.SetInt("ScreenRate", screenrate.value);
        PlayerPrefs.SetInt("ResolutionIndex", resolution.value);
        PlayerPrefs.SetInt("GraphicIndex", graphic.value);
        PlayerPrefs.SetInt("DisplayIndex", display.value);
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
        PlayerPrefs.Save();

        // 현재 상태를 새로운 기준으로 업데이트
        _originalBrightness = brightSlider.value;
        _originalScreenRateIndex = screenrate.value;
        _originalResolutionIndex = resolution.value;
        _originalGraphicIndex = graphic.value;
        _originalDisplayIndex = display.value;
        _originalSensitivity = sensitivitySlider.value;

        saveBtn.gameObject.SetActive(false);
    }

    public void OnClickOKBtn()
    {
        PlayerPrefs.SetFloat("Brightness", brightSlider.value);
        PlayerPrefs.SetInt("ScreenRate", screenrate.value);
        PlayerPrefs.SetInt("ResolutionIndex", resolution.value);
        PlayerPrefs.SetInt("GraphicIndex", graphic.value);
        PlayerPrefs.SetInt("DisplayIndex", display.value);
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
        PlayerPrefs.Save();

        _originalBrightness = brightSlider.value;
        _originalScreenRateIndex = screenrate.value;
        _originalResolutionIndex = resolution.value;
        _originalGraphicIndex = graphic.value;
        _originalDisplayIndex = display.value;
        _originalSensitivity = sensitivitySlider.value;

        this.gameObject.SetActive(false);
    }

    public void OnClickCancelBtn()
    {
        this.gameObject.SetActive(false);
    }
}
