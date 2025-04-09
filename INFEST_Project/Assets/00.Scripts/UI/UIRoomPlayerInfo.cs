using Fusion;

public class UIRoomPlayerInfo : NetworkBehaviour
{
    public bool IsSetted = false;
    public TMPro.TextMeshProUGUI NickName;
    public TMPro.TextMeshProUGUI Job;

    public void OnPlayerEnter(string nickName, string job)
    {
        IsSetted = true;
        NickName.text = nickName;
        Job.text = job;
    }

    public void OnPlayerExit()
    {
        IsSetted = false;
        NickName.text = string.Empty;
        Job.text = string.Empty;
    }
}
