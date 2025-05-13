using UnityEngine;

public class CharacterInfoInstance
{
    public readonly CharacterInfo data;

    public CharacterInfoInstance(int key)
    {
        data = DataManager.Instance.GetByKey<CharacterInfo>(key);
    }

}
