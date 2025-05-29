
using System.Collections.Generic;
using UnityEngine;

public class Monster_RageFang_Phase_Retreat : MonsterPhase<Monster_RageFang>
{
    public Vector3 targetPosition;
    public Transform t;
    public float speed;
    public LayerMask obstacleLayer;
    public bool FirstJump = false;

    public override void MachineEnter()
    {
        base.MachineEnter();

        monster.AIPathing.enabled = false;
        targetPosition = new Vector3(monster.nextRegion[monster.regionIndex].PointX, monster.nextRegion[monster.regionIndex].PointY, monster.nextRegion[monster.regionIndex].PointZ);
        speed = 5f; // 원하는 속도
        t = transform.root;
        t.forward = (targetPosition - t.position).normalized;
        monster.regionIndex++;
        ChangeState<Monster_RageFang_Retreat_Roaring>();
        monster.IsReadyForChangingState = false;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.IsReadyForChangingState)
        {
            ChangeState<Monster_RageFang_Retreat_Jump>();
        }
    }

    public override void MachineExit()
    {
        base.MachineExit();
        monster.AIPathing.enabled = true;
        monster.ClearTargetList();
    }
}
