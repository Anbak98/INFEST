using UnityEngine;

public class FSM_PJ_HI : MonsterFSM<Monster_PJ_HI>
{
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if(monster.IsLookPlayer())
        {
            ChangePhase<PJ_HI_Phase_Chase>();
        }
    }
}
