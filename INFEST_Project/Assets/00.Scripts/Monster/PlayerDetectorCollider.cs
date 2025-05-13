using UnityEngine;

public class PlayerDetectorCollider : MonoBehaviour
{
    public MonsterNetworkBehaviour monster;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
                monster.TryAddTarget(other.transform);
                monster.SetTargetRandomly();
        }
    }
    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            monster.TryRemoveTarget(other.transform);
        }
    }
}
