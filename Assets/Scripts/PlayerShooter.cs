using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//할거 UI갱신
//애니메이터 IK갱신


public class PlayerShooter : MonoBehaviour
{
    //공격 딜레이에 따른 애니메이션 재생 때문에 가져옴
    public Bow bow;

    public Transform bowPivot; // 활 배치의 기준점
    public Transform leftHandMount;  // 활의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 활의 오른쪽 손잡이, 오른손이 위치할 지점

    private Joystick joystick;

    //활 쏘기 모션을 위한 애니메이터
    private Animator playerAnimator;

    public float attackSpeed = 1f; //공격 속도

    private float lastFireTime; //마지막 발사 시점
    private bool isAttacking = false; //공격 중인지
    private float attackDelay; // 공격 딜레이

    void Start()
    {
        //Joystick의 인스턴스 가져오기
        joystick = Joystick.instance;
        if(joystick == null) {
            Debug.LogError("Joystick 인스턴스를 찾을 수 없습니다.");
        }

        //플레이어의 Animator 컴포넌트 가져오기
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable() {
        //공격 시점 초기화
        lastFireTime = 0f;
        //공격 딜레이 동기화
        attackDelay = bow.bowData.timeBetFire;
        //PlayerShooter가 활성화 될 때 활도 함게 활성화
        bow.gameObject.SetActive(true);
    }

    private void OnDisable() {
        //PlayerShooter가 비활성화 될 때 활도 함게 비활성화
        bow.gameObject.SetActive(false);
    }

    void Update()
    {
        //조이스틱을 누르고 있지 않고 && 마지막 발사시간 + 공격 딜레이의 시간이 지나면
        if(joystick.onPointer != false && Time.time >= lastFireTime + attackDelay) {

            //마지막 발사 시간 갱신
            lastFireTime = Time.time;

            //누르고 있을 때의 bool 값을 넘겨 애니메이션 재생
            playerAnimator.SetBool("Attack", joystick.onPointer);

            //StartAttack() 메서드 호출
            StartAttack();
        }
        
        //조이스틱을 누르고 있다면
        else if(joystick.onPointer != true) {

        //안 누르고 있을 때의 bool 값을 넘겨 애니메이션 멈춤
           playerAnimator.SetBool("Attack", joystick.onPointer);
        }
    }


    private void StartAttack() {
        
        //공격 중이니 활성화
        isAttacking = true;

        //현재 진행중인 상태의 애니메이션 정보 가져옴
        //AnimatorStateInfo shotAnimationInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        //playerAnimator.speed = attackSpeed;

        //코루틴 시작
        StartCoroutine(WaitForAttackEnd());

    }


    private IEnumerator WaitForAttackEnd() {

        //애니메이션의 재생 시간을 가져오는 메서드
        //BowShot 모션의 재생시간을 가져옴 
        float animationLength = GetAnimationClipLength("BowShot");


        //공격 딜레이 = 공격모션 재생시간 / 공격 속도
        //공격 속도가 오를수록 딜레이는 짧아짐 ex) 공속100%증가 = 딜레이 50%  반비례임
        attackDelay = animationLength / attackSpeed;
        Debug.Log("playtime : " + attackDelay);
        //공격 딜레이 시간만큼 코루틴 정지
        yield return new WaitForSeconds(attackDelay);

        //실제 공격 메서드
        OnAttackComplete();
    }

    private void OnAttackComplete() {
        //공격!
        bow.Fire();
        //공격 끝났으니 공격중 bool 값 비활성화
        isAttacking = false;

    }

    //애니메이션(ClipName)의 재생 시간을 가져오는 메서드
    private float GetAnimationClipLength(string clipName) {
        //playerAnimator가 존재하는지 확인
        if (playerAnimator != null) {
            //애니메이터에 포함된 모든 클립을 가져옴
            AnimationClip[] clips = playerAnimator.runtimeAnimatorController.animationClips;
            //clips 배열에 있는 각 애니메이션 클립을 순회
            foreach (AnimationClip clip in clips) {
                //clipName 과 동일한 클립을 가져와서
                if (clip.name == clipName) {
                    Debug.Log("length : "+clip.length);
                    //해당 클립의 길이(초 단위)를 반환
                    return clip.length;
                }
            }
        }
        return 0f;
    }

    //공격 속도 변화 및 애니메이션 재생 속도를 변경하는 메서드
    public void SetAttackSpeed(float value) {
        //value 값만큼 공격 속도 증가 (% 단위)
        attackSpeed = (1 + (float)(0.01 * value));
        //증가된 공격 속도만큼 애니메이션 재생 속도 증가
        SetAnimationSpeed("BowShot", attackSpeed);
    }

    // 특정 애니메이션의 재생 속도 설정하는 메서드
    private void SetAnimationSpeed(string clipName, float speed) {
        // 파라미터 이름 생성
        string speedParameterName = clipName + "Speed";

        // 파라미터가 존재하는지 확인
        if (HasParameter(speedParameterName)) {
            // 재생 속도 설정
            playerAnimator.SetFloat(speedParameterName, speed);
        }
        else {
            Debug.LogWarning($"파라미터 {speedParameterName}를 찾을 수 없습니다.");
        }
    }

    // Animator에 특정 파라미터가 있는지 확인하는 메서드
    private bool HasParameter(string paramName) {
        //playerAnimator의 파라미터를 순회
        foreach (AnimatorControllerParameter param in playerAnimator.parameters) {
            //paramName과 일치하는 파라미터가 있으면 
            if (param.name == paramName) {
                //true 반환
                return true;
            }
        }
        //paramName 과 일치하는 파라미터가 없으면 false 반환
        return false;
    }

    //애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex) {

        //오른손을 이동으로 시작해보려함
    }



}
