using System.Collections;
using UnityEngine;

public class UIExitPopup : UIScreen
{
    public void OKBtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void CancelBtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        gameObject.SetActive(false);
    }
}
