using Fusion;
using UnityEngine;

public class TestHp : NetworkBehaviour
{
    public float maxHealth = 100f;
    public float immortalDurationAfterSpawn = 2f;
    //public GameObject immortalityIndicator;
    //public GameObject hitEffectPrefab;

    public bool isAlive => CurrentHealth > 0f;
    public bool isImmortal => ImmortalTimer.ExpiredOrNotRunning(Runner) == false;

    [Networked]
    public float CurrentHealth { get; private set; }

    [Networked]
    private int HitCount { get; set; }
    [Networked]
    private Vector3 LastHitPosition { get; set; }
    [Networked]
    private Vector3 LastHitDirection { get; set; }
    [Networked]
    private TickTimer ImmortalTimer { get; set; }

    private int _visibleHitCount;
    //private SceneObjects _sceneObjects;

    public bool ApplyDamage(PlayerRef instigator, float damage, Vector3 position, Vector3 direction, EWeaponType weaponType, bool isCritical)
    {
        if (CurrentHealth <= 0f)
            return false;

        if (isImmortal)
            return false;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;

            //_sceneObjects.Gameplay.PlayerKilled(instigator, Object.InputAuthority, weaponType, isCritical);
        }

        // Store relative hit position.
        // Only last hit is stored. For casual gameplay this is enough, no need to store precise data for each hit.
        LastHitPosition = position - transform.position;
        LastHitDirection = -direction;

        HitCount++;

        return true;
    }

    public bool AddHealth(float health)
    {
        if (CurrentHealth <= 0f)
            return false;
        if (CurrentHealth >= maxHealth)
            return false;

        CurrentHealth = Mathf.Min(CurrentHealth + health, maxHealth);

        if (HasInputAuthority && Runner.IsForward)
        {
            // Heal effect is shown only to local player.
            // We assume the prediction will be correct most of the time so we don't need to network anything explicitly.
            //_sceneObjects.GameUI.PlayerView.Health.ShowHeal(health);
        }

        return true;
    }

    public void StopImmortality()
    {
        ImmortalTimer = default;
    }

    public override void Spawned()
    {
        //_sceneObjects = Runner.GetSingleton<SceneObjects>();

        if (HasStateAuthority)
        {
            CurrentHealth = maxHealth;

            ImmortalTimer = TickTimer.CreateFromSeconds(Runner, immortalDurationAfterSpawn);
        }

        _visibleHitCount = HitCount;
    }

    public override void Render()
    {
        if (_visibleHitCount < HitCount)
        {
            // Network hit counter changed in FUN, play damage effect.
            //PlayDamageEffect();
        }

        //immortalityIndicator.SetActive(isImmortal);

        // Sync network hit counter with local.
        _visibleHitCount = HitCount;
    }

    //private void PlayDamageEffect()
    //{
    //    if (hitEffectPrefab != null)
    //    {
    //        var hitPosition = transform.position + LastHitPosition;
    //        var hitRotation = Quaternion.LookRotation(LastHitDirection);

    //        Instantiate(hitEffectPrefab, hitPosition, hitRotation);
    //    }
    //}
}
