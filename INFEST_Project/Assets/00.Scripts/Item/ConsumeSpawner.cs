using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ConsumeSpawner : NetworkBehaviour
{
    public List<Consume> Consumes;

    public void Throw()
    {
        for(int i=0; i<3; i++)
        {
            if (Consumes[i] == Player.local.inventory.consume[0])
            {
                Consumes[i].CollThrow();
            }
        }
    }
}
