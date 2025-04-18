using UnityEngine.UI;

public class ConsumeInstance
{
    public readonly ConsumeItem data;

    public ConsumeInstance(int key)
    {
        data = DataManager.Instance.GetByKey<ConsumeItem>(key);
    }

    public float curNum { get; private set; } = 0;                              // 현재 아이템 갯수
    public Image icon { get; set; }                                             // UI에 사용될 아이콘

    public void AddNum()
    {
        curNum++;
    }
}
