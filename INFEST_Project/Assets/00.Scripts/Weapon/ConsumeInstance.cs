using UnityEngine.UI;

public class ConsumeInstance
{
    public readonly ConsumeItem data;

    public ConsumeInstance(int key)
    {
        data = DataManager.Instance.GetByKey<ConsumeItem>(key);
    }

    public float curNum { get; private set; } = 1;                              // ���� ������ ����
    public Image icon { get; set; }                                             // UI�� ���� ������

    public void AddNum()
    {
        curNum++;
    }
    public void RemoveNum()
    {
        curNum--;
    }
}
