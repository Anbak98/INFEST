using Fusion;
using UnityEngine;

public class PJ_HI_Phase_Wave : MonsterPhase<Monster_PJ_HI>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
    }

    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.PlayerDetectorCollider.radius != monster.info.DetectAreaWave)
            monster.PlayerDetectorCollider.radius = monster.info.DetectAreaWave;
    }

    public override void MachineExit()
    {
        monster.PlayerDetectorCollider.radius = monster.info.DetectAreaNormal;
        base.MachineExit();
    }
}