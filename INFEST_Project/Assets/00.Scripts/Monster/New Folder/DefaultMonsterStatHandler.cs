using Fusion;
using INFEST.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Infest.Monster
{
    public class DefaultMonsterStatHandler : IMonsterStatHandler
    {
        public MonsterInfo Info { get; private set; }

        public MonsterStatus Health { get; private set; }

        public MonsterStatus Damage { get; private set; }

        public MonsterStatus Defend { get; private set; }

        [field: Header("Monster Status")]
        [Networked, OnChangedRender(nameof(OnChangedMovementSpeed))] public float CurMovementSpeed { get; set; }
        [Networked, OnChangedRender(nameof(OnChangedDetectorRadiusSpeed))] public float CurDetectorRadius { get; set; }

        public void Init(int key, int playerCount, int elapsedTime)
        {
            Info = DataManager.Instance.GetByKey<MonsterInfo>(key);

            Health = new MonsterStatus((int)(Info.MinHealth * (1 + Info.HPCoef * playerCount)), (int)(Info.HealthPer5Min * elapsedTime));
            Damage = new MonsterStatus((int)(Info.MinAtk * (1 + Info.AtkCoef * playerCount)), (int)(Info.AtkPer5Min * elapsedTime));
            Defend = new MonsterStatus((int)(Info.MinDef * (1 + Info.DefCoef * playerCount)), (int)(Info.DefPer5Min * elapsedTime));
        }

        public virtual bool ApplyDamage(PlayerRef instigator, float damage, Vector3 position, Vector3 direction, EWeaponType weaponType, bool isCritical)
        {
            if (isCritical)
                Debug.Log("HeadShot");
            if (CurHealth <= 0f)
                return false;

            int dmg = (int)damage - CurDef;

            if (dmg <= 0)
                dmg = 1;

            CurHealth -= dmg;

            if (CurHealth <= 0f)
            {
                CurHealth = 0;
                NetworkGameManager.Instance.gamePlayers.AddKillCount(instigator, 1);
                NetworkGameManager.Instance.gamePlayers.AddGoldCount(instigator, info.DropGold);


                IsDead = true;

                if (weaponType == EWeaponType.Launcher)
                {
                    RPC_RagdollEffect(position);
                }
            }

            Vector3 dir = (transform.position - position).normalized;

            // Store relative hit position.
            // Only last hit is stored. For casual gameplay this is enough, no need to store precise data for each hit.

            RPC_PlayDamageEffect(position - transform.position, -dir);

            return true;
        }

        private void OnChangedMovementSpeed()
        {
            animator.SetFloat("MovementSpeed", CurMovementSpeed);
            AIPathing.speed = CurMovementSpeed;
        }

        private void OnChangedDetectorRadiusSpeed()
        {
            PlayerDetectorCollider.radius = CurDetectorRadius;
        }

    }
}