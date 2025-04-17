using UnityEngine.UI;

public class ConsumeInstance
{
    public readonly ConsumeItem data;

    public ConsumeInstance(int key)
    {
        data = DataManager.Instance.ConsumeItemLoader.GetByKey(key);
    }

    public float curNum { get; private set; } = 0;                              // ���� ������ ����
    public Image icon { get; set; }                                             // UI�� ���� ������

    public void AddNum()
    {
        curNum++;
    }
}
