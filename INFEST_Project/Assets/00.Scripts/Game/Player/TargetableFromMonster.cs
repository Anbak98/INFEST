using UnityEngine;

public class TargetableFromMonster : MonoBehaviour
{
    [SerializeField] private PlayerStatHandler playerStatHandler;

    public void ApplyDamage(int monsterKey, int damage)
    {
        playerStatHandler.TakeDamage(damage);
    }
}
