using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    //���̽�ƽ ����
    public Joystick joystick;
    //�÷��̾� �̵� �ӵ�
    public float moveSpeed = 5f;

    //�÷��̾��� ������ٵ�
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    void Start() {
        //�÷��̾� ������ٵ� ������Ʈ ��������
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }



    private void FixedUpdate() {
        Rotate();
        Move();
        Debug.Log(joystick.Vertical);
        playerAnimator.SetFloat("Move", joystick.Direction.magnitude);
        playerAnimator.SetBool("Attack", joystick.onPointer);

    }

    private void Move() {
        //���̽�ƽ�� �Է� ���� ���ͷ� ����
        Vector3 direction = new Vector3(joystick.Direction.x, 0 , joystick.Direction.y);
        //�Է� ���� ���� �÷��̾� �̵�
        playerRigidbody.velocity = direction * moveSpeed;
       
    }

    private void Rotate() {
        //���̽�ƽ�� ���� ������ ���ؼ� ȸ������ ����.
        float turn = Mathf.Atan2(joystick.Direction.x, joystick.Direction.y) * Mathf.Rad2Deg;
        //���̽�ƽ�� ���� �Է� ���� ���� ��쿡�� ȸ��
        if (joystick.Direction.x != 0 || joystick.Direction.y != 0) {
            transform.rotation = Quaternion.Euler(0, turn, 0);
        }
    }

}
