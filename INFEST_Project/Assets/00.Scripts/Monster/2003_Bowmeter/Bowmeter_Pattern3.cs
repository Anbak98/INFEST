using Fusion;
using UnityEngine;

public class Bowmeter_Pattern3 : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
{
    public VomitAndArea vomitAndArea;    

    public override void Enter()
    {
        base.Enter();
        if (monster.IsDead || monster.target == null)
            return;

        monster.IsShoot = true;
        monster.CurMovementSpeed = 0f;        

        phase.skillCoolDown[3] = TickTimer.CreateFromSeconds(Runner, monster.skills[3].CoolDown);
    }

    public override void Execute()
    {
        base.Execute();

        if (monster.target != null)
        {
            Vector3 dir = (monster.target.position - monster.transform.position).normalized;
            dir.y = 0f;
            Quaternion targetRot = Quaternion.LookRotation(dir);
            monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, targetRot, Time.deltaTime * 5f);
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsShoot = false;
    }
    public override void Effect()
    {
        base.Effect();

        if (monster.target != null)
        {
            AudioManager.instance.PlaySfx(Sfxs.BowmeterAttack1);
            Vector3 vomitPos = phase.vomitPosition.position;
            Vector3 targetPos = monster.target.position;

            Vector3 direction = (targetPos - vomitPos).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            var spawnedVomit = Runner.Spawn(vomitAndArea, vomitPos, lookRotation);

            if (spawnedVomit != null)
            {
                spawnedVomit.GetComponent<VomitAndArea>().ownerPattern3 = this;
            }
        }
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage * monster.skills[3].DamageCoefficient));
    }
}
