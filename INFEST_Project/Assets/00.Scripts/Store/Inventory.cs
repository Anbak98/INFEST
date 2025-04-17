using Fusion;

public class Inventory
{

    public int[] weapon = new int[2];
    public int[] auxiliaryWeapon = new int[1];
    public int[] consume = new int[3];
    public int[] inventoryItem = new int[3];

    public void AddWeponItme(WeaponInstance item)
    {
        if (item.data.key % 10000 > 200)
        {
            if (weapon[0] == 0)
                weapon[0] = item.data.key;

            else
                weapon[1] = item.data.key;
        }
        else
        {
            auxiliaryWeapon[0] = item.data.key;
        }
    }

    public void AddConsumeItme(ConsumeInstance item)
    {
        if (item.data.key % 10000 < 800)
            consume[0] = item.data.key;

        else if (item.data.key % 10000 < 900)
            consume[1] = item.data.key;

        else
            consume[2] = item.data.key;
    }

    public void RemoveWeapon(WeaponInstance item, int index)
    {
        if (item.data.key % 10000 > 200)
            weapon[index] = 0;
        else
            auxiliaryWeapon[0] = 0;
    }
    public void RemoveConsume(int index)
    {
        consume[index] = 0;
    }
}
