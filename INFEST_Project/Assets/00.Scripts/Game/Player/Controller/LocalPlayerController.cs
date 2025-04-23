using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 1인칭 프리팹에 붙어서 애니메이션 관리
/// 1인칭 프리팹의 애니메이션, 회전을 관리한다
/// 
/// 마우스 커서 이동
/// </summary>
public class LocalPlayerController : PlayerController
{
    // 1인칭에 적용되는 무기의 animator는 3종류이며 무기가 바뀌면 animator를 교체해야한다
    public Animator animator;

    public override void Awake()
    {
        // animator이 있는 곳에 추가했다(1인칭, 3인칭 각각)
        animator = GetComponent<Animator>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// 1인칭은 나만 가지고 있는거니까 상태 변화하는건 자신의 Update에서 처리한다
    /// </summary>
    // Update is called once per frame
    public override void Update()
    {
        
    }

}
