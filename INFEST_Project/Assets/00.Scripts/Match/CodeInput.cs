using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CodeInput : MonoBehaviour
{
    public TextMeshProUGUI codeDisplayText;
    private List<char> inputChars = new List<char>();
    private int codeLength = 6;

    private bool showCursor = true;
    private float cursorBlinkRate = 0.5f; // 초당 깜빡임 속도
    private Coroutine blinkCoroutine;

    void Start()
    {
        UpdateDisplay();
        blinkCoroutine = StartCoroutine(BlinkCursor());
    }

    void Update()
    {
        if (inputChars.Count < codeLength)
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsLetterOrDigit(c))
                {
                    inputChars.Add(c);
                    if (inputChars.Count >= codeLength) break;
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && inputChars.Count > 0)
            {
                inputChars.RemoveAt(inputChars.Count - 1);
            }

            // ✅ Ctrl+V or Cmd+V 붙여넣기 처리
#if UNITY_STANDALONE || UNITY_EDITOR
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                 Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand)) &&
                Input.GetKeyDown(KeyCode.V))
            {
                string paste = GUIUtility.systemCopyBuffer;
                PasteFromClipboard(paste);
            }
#endif
        }

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        string display = "";

        for (int i = 0; i < codeLength; i++)
        {
            if (i < inputChars.Count)
            {
                display += inputChars[i];
            }
            else if (i == inputChars.Count && showCursor)
            {
                display += "|"; // 깜빡이는 커서
            }
            else
            {
                display += "_";
            }
        }

        codeDisplayText.text = display;
    }

    IEnumerator BlinkCursor()
    {
        while (true)
        {
            showCursor = !showCursor;
            UpdateDisplay();
            yield return new WaitForSeconds(cursorBlinkRate);
        }
    }

    public void ClearInput()
    {
        inputChars.Clear();
        UpdateDisplay();
    }

    public string GetCode()
    {
        return new string(inputChars.ToArray());
    }

    public void OnPressedJoinButton()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UILoadingPopup>();
        MatchManager.Instance.JoinSession(codeDisplayText.text);
    }

    void PasteFromClipboard(string clipboardText)
    {
        foreach (char c in clipboardText)
        {
            if (char.IsLetterOrDigit(c))
            {
                inputChars.Add(c);
                if (inputChars.Count >= codeLength)
                    break;
            }
        }
    }
}
