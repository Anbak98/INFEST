using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class Shield : Consume
{
    private TickTimer _shieldTimer;
    public NetworkPrefabRef mountingPrefab;
    public Transform mountingPoint;
    

    public override void Mounting()
    {
        Debug.Log("Shield »£√‚");

        if (!_shieldTimer.ExpiredOrNotRunning(Runner)) return;

        //_player.inventory.RemoveConsumeItem(2);

        ShieldCreate();
    }

    private void ShieldCreate()
    {
        //Vector3 direction = _player.transform.forward;
        //Quaternion rotation = Quaternion.LookRotation(direction);

        if (Object.HasStateAuthority)
        {
            Quaternion baseRotation = Quaternion.LookRotation(_player.transform.forward);
            Quaternion offsetRotation = Quaternion.Euler(0, 90f, 0);
            Quaternion finalRotation = baseRotation * offsetRotation;

            Vector3 createPosition = mountingPoint.position;
            createPosition.y = 0f;

            Runner.Spawn(
            mountingPrefab,
            createPosition,
            finalRotation,
            Object.InputAuthority
            );

            
        }
        _shieldTimer = TickTimer.CreateFromSeconds(Runner, 0.5f);
    }

}
