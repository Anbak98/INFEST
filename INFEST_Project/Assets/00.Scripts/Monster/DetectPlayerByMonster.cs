using UnityEngine;

public class DetectPlayerByMonster : MonoBehaviour
{
    [SerializeField] private MonsterNetworkBehaviour monster; 

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            monster.target = other.gameObject.transform;
            monster.targetStatHandler = other.GetComponent<PlayerStatHandler>();
            monster.FSM.ChangePhase<PJ_HI_ChasePhase>();
        }
    }
}
