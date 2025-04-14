using Fusion;

public class Inventory : NetworkBehaviour
{
    public int[] inventoryItme = new int[6];
    
    public void AddWeponItme(WeaponInstance item)
    {
        if (item.data.key % 10000 > 200)
        {
            if (inventoryItme[0] == 0)
                inventoryItme[0] = item.data.key;

            else
                inventoryItme[1] = item.data.key;
        }
        else
        {
            inventoryItme[2] = item.data.key;
        }
    }

    public void AddConsumeItme(ConsumeInstance item)
    {
        if (item.data.key % 10000 < 800)
            inventoryItme[3] = item.data.key;

        else if (item.data.key % 10000 < 900)
            inventoryItme[4] = item.data.key;

        else
            inventoryItme[5] = item.data.key;
    }

    public void RemoveItem(int index)
    {
        inventoryItme[index] = 0;
    }
}
