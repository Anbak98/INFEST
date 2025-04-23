public class Monster_PJ_HI : MonsterNetworkBehaviour
{
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            target = other.gameObject.transform;
            FSM.ChangePhase<PJ_HI_ChasePhase>();
        } 
    }
}
