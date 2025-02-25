using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ұ� UI����
//�ִϸ����� IK����


public class PlayerShooter : MonoBehaviour
{
    //���� �����̿� ���� �ִϸ��̼� ��� ������ ������
    public Bow bow;

    public Transform bowPivot; // Ȱ ��ġ�� ������
    public Transform leftHandMount;  // Ȱ�� ���� ������, �޼��� ��ġ�� ����
    public Transform rightHandMount; // Ȱ�� ������ ������, �������� ��ġ�� ����

    private Joystick joystick;

    //Ȱ ��� ����� ���� �ִϸ�����
    private Animator playerAnimator;

    public float attackSpeed = 1f; //���� �ӵ�

    private float lastFireTime; //������ �߻� ����
    private bool isAttacking = false; //���� ������
    private float attackDelay; // ���� ������

    void Start()
    {
        //Joystick�� �ν��Ͻ� ��������
        joystick = Joystick.instance;
        if(joystick == null) {
            Debug.LogError("Joystick �ν��Ͻ��� ã�� �� �����ϴ�.");
        }

        //�÷��̾��� Animator ������Ʈ ��������
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable() {
        //���� ���� �ʱ�ȭ
        lastFireTime = 0f;
        //���� ������ ����ȭ
        attackDelay = bow.bowData.timeBetFire;
        //PlayerShooter�� Ȱ��ȭ �� �� Ȱ�� �԰� Ȱ��ȭ
        bow.gameObject.SetActive(true);
    }

    private void OnDisable() {
        //PlayerShooter�� ��Ȱ��ȭ �� �� Ȱ�� �԰� ��Ȱ��ȭ
        bow.gameObject.SetActive(false);
    }

    void Update()
    {
        //���̽�ƽ�� ������ ���� �ʰ� && ������ �߻�ð� + ���� �������� �ð��� ������
        if(joystick.onPointer != false && Time.time >= lastFireTime + attackDelay) {

            //������ �߻� �ð� ����
            lastFireTime = Time.time;

            //������ ���� ���� bool ���� �Ѱ� �ִϸ��̼� ���
            playerAnimator.SetBool("Attack", joystick.onPointer);

            //StartAttack() �޼��� ȣ��
            StartAttack();
        }
        
        //���̽�ƽ�� ������ �ִٸ�
        else if(joystick.onPointer != true) {

        //�� ������ ���� ���� bool ���� �Ѱ� �ִϸ��̼� ����
           playerAnimator.SetBool("Attack", joystick.onPointer);
        }
    }


    private void StartAttack() {
        
        //���� ���̴� Ȱ��ȭ
        isAttacking = true;

        //���� �������� ������ �ִϸ��̼� ���� ������
        //AnimatorStateInfo shotAnimationInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        //playerAnimator.speed = attackSpeed;

        //�ڷ�ƾ ����
        StartCoroutine(WaitForAttackEnd());

    }


    private IEnumerator WaitForAttackEnd() {

        //�ִϸ��̼��� ��� �ð��� �������� �޼���
        //BowShot ����� ����ð��� ������ 
        float animationLength = GetAnimationClipLength("BowShot");


        //���� ������ = ���ݸ�� ����ð� / ���� �ӵ�
        //���� �ӵ��� �������� �����̴� ª���� ex) ����100%���� = ������ 50%  �ݺ����
        attackDelay = animationLength / attackSpeed;
        Debug.Log("playtime : " + attackDelay);
        //���� ������ �ð���ŭ �ڷ�ƾ ����
        yield return new WaitForSeconds(attackDelay);

        //���� ���� �޼���
        OnAttackComplete();
    }

    private void OnAttackComplete() {
        //����!
        bow.Fire();
        //���� �������� ������ bool �� ��Ȱ��ȭ
        isAttacking = false;

    }

    //�ִϸ��̼�(ClipName)�� ��� �ð��� �������� �޼���
    private float GetAnimationClipLength(string clipName) {
        //playerAnimator�� �����ϴ��� Ȯ��
        if (playerAnimator != null) {
            //�ִϸ����Ϳ� ���Ե� ��� Ŭ���� ������
            AnimationClip[] clips = playerAnimator.runtimeAnimatorController.animationClips;
            //clips �迭�� �ִ� �� �ִϸ��̼� Ŭ���� ��ȸ
            foreach (AnimationClip clip in clips) {
                //clipName �� ������ Ŭ���� �����ͼ�
                if (clip.name == clipName) {
                    Debug.Log("length : "+clip.length);
                    //�ش� Ŭ���� ����(�� ����)�� ��ȯ
                    return clip.length;
                }
            }
        }
        return 0f;
    }

    //���� �ӵ� ��ȭ �� �ִϸ��̼� ��� �ӵ��� �����ϴ� �޼���
    public void SetAttackSpeed(float value) {
        //value ����ŭ ���� �ӵ� ���� (% ����)
        attackSpeed = (1 + (float)(0.01 * value));
        //������ ���� �ӵ���ŭ �ִϸ��̼� ��� �ӵ� ����
        SetAnimationSpeed("BowShot", attackSpeed);
    }

    // Ư�� �ִϸ��̼��� ��� �ӵ� �����ϴ� �޼���
    private void SetAnimationSpeed(string clipName, float speed) {
        // �Ķ���� �̸� ����
        string speedParameterName = clipName + "Speed";

        // �Ķ���Ͱ� �����ϴ��� Ȯ��
        if (HasParameter(speedParameterName)) {
            // ��� �ӵ� ����
            playerAnimator.SetFloat(speedParameterName, speed);
        }
        else {
            Debug.LogWarning($"�Ķ���� {speedParameterName}�� ã�� �� �����ϴ�.");
        }
    }

    // Animator�� Ư�� �Ķ���Ͱ� �ִ��� Ȯ���ϴ� �޼���
    private bool HasParameter(string paramName) {
        //playerAnimator�� �Ķ���͸� ��ȸ
        foreach (AnimatorControllerParameter param in playerAnimator.parameters) {
            //paramName�� ��ġ�ϴ� �Ķ���Ͱ� ������ 
            if (param.name == paramName) {
                //true ��ȯ
                return true;
            }
        }
        //paramName �� ��ġ�ϴ� �Ķ���Ͱ� ������ false ��ȯ
        return false;
    }

    //�ִϸ������� IK ����
    private void OnAnimatorIK(int layerIndex) {

        //�������� �̵����� �����غ�����
    }



}
