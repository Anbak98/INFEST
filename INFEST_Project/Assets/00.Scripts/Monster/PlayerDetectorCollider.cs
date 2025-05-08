using UnityEngine;

public class PlayerDetectorCollider : MonoBehaviour
{
    public MonsterNetworkBehaviour monster;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if (!monster.targets.Contains(other.transform))
                monster.targets.Add(other.transform);
        }
    }
    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if (monster.targets.Contains(other.transform))
                monster.targets.Remove(other.transform);
        }
    }
}
