using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitMatching : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_WEBGL
        Global.Instance.UIManager.Show<UISetProfile>();   
#endif
        PlayerPrefsManager.SetNickname("Web User");
        MatchManager.Instance.RoomUI.UpdateUI(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
