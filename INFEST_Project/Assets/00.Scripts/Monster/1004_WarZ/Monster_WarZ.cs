using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_WarZ : BaseMonster<Monster_WarZ>
{
    public TickTimer animTickTimer;   // 애니메이션의 재생시간동안 다른 동작으로 못바꾼다(특히 공격)

    //[Networked, OnChangedRender(nameof(OnIsHeadButt))]
    //public NetworkBool IsHeadButt { get; set; } = false;


    [Networked, OnChangedRender(nameof(OnIsRunning))]
    public NetworkBool IsRunning { get; set; } = false;



    [Networked, OnChangedRender(nameof(OnIsWave))]
    public NetworkBool IsWave { get; set; } = false;

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
        animator.SetFloat("Distance", AIPathing.remainingDistance);

        if (IsAttack)
        {
            animator.SetTrigger("IsAttack");
        }
        if (IsDead)
        {
            animator.SetBool("IsDead", IsDead);
        }
        AIPathing.speed = CurMovementSpeed;
    }
    //private void OnIsHeadButt() => animator.SetBool("IsHeadButt", IsHeadButt);


    // 처음 상태 애니메이션 재생
    //private void OnIsIdle() => animator.Play("Wander.DeadCop_Idle");
    private void OnIsRunning() => animator.Play("Wander.WarZ_Run");


    // 웨이브 시작, 종료조건 
    private void OnIsWave() => animator.SetBool("IsWave", IsWave);  // 웨이브 생성과 관련된 곳에서 가져와서 bool값을 바꾼다




    public float GetCurrentAnimLength()
    {
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0)
        {
            return clipInfos[0].clip.length;
        }
        return 0f;
    }
}
