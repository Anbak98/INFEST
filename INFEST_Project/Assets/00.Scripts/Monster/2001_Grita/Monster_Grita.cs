using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Monster_Grita : BaseMonster<Monster_Grita>
{
    [SerializeField] private AudioSource _screamSound;
    [SerializeField] private AudioClip _screamSoundClip;


    public const int screamMaxCount = 2;  // 최대 2번만 가능하다
    public int screamCount = 0;  // 최대 2번만 가능하다

    public TickTimer screamCooldownTimer;
    public const float ScreamCooldownSeconds = 50f;

    public GritaPlayerDetector playerDetector; // 연결 필요

    // 동적으로 연결
    [SerializeField] public MonsterSpawner spawner;


    #region animotor parameter
    //[Networked, OnChangedRender(nameof(OnLookXChanged))]
    //public float LookX { get; set; } = 0f;

    [Networked, OnChangedRender(nameof(OnIsScreamChanged))]
    public NetworkBool IsScream { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsCooltimeCharged))]
    public NetworkBool IsCooltimeCharged { get; set; } = true;
    [Networked, OnChangedRender(nameof(OnScreamCount))]
    public int ScreamCount { get; set; } = 0;
    

    //private void OnLookXChanged() => animator.SetFloat("LookX", LookX);
    private void OnIsScreamChanged() => animator.SetBool("IsScream", IsScream);
    private void OnIsCooltimeCharged() => animator.SetBool("IsCooltimeCharged", IsCooltimeCharged);
    private void OnScreamCount() => animator.SetInteger("ScreamCount", ScreamCount);
    #endregion



    public override void Render()
    {
        // Animation 관련
        animator.SetFloat("MovementSpeed", MovementSpeed);
        if (IsAttack)
            animator.SetTrigger("IsAttack");
        if (IsDead)
            animator.SetBool("IsDead", IsDead);
        AIPathing.speed = MovementSpeed;

        //// 타겟이 존재할 경우 LookX 계산
        //if (target != null)
        //{
        //    Vector3 toTarget = (target.position - transform.position).normalized;
        //    toTarget.y = 0; // 수직 무시

        //    // forward로부터의 좌우 각도: 90도에서 절댓값 1, 정면/정반대에서 0
        //    LookX = Vector3.Dot(transform.right, toTarget);
        //    LookX = Mathf.Clamp(LookX, -1f, 1f);
        //    animator.SetFloat("LookX", LookX);
        //}

    }


    public bool CanScream()
    {
        return screamCooldownTimer.ExpiredOrNotRunning(Runner);
    }

    // 소리는 Host가 모든 Player로 쏴 주는 RPC 
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_Scream()
    {
        Debug.Log("Scream!");   // 나중에 지우고 커밋

        if (_screamSound != null && _screamSoundClip != null)
            _screamSound.PlayOneShot(_screamSoundClip);

        screamCount++;

        // 쿨타임 시작
        screamCooldownTimer = TickTimer.CreateFromSeconds(Runner, ScreamCooldownSeconds);
    }
    #region 쿨타임 후 isTriggered를 false로
    public void StartScreamCooldown()
    {
        playerDetector.isTriggered = true;
        // 50초 후에 ResetTrigger 함수 자동 호출
        Invoke(nameof(ResetTrigger), ScreamCooldownSeconds);
    }
    private void ResetTrigger()
    {
        playerDetector.isTriggered = false;
        IsCooltimeCharged = true;  
        Debug.Log("쿨타임 종료: isTriggered = false");
    }
    #endregion

    public float GetCurrentAnimLength()
    {
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0)
        {
            return clipInfos[0].clip.length;
        }
        return 0f;
    }
    public IEnumerator SpawnAfterAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("SpawnAfterAnim 실행");

        spawner.SpawnMonsterOnWave(spawner.transform);

    }


}
