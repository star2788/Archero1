using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    //조이스틱 에셋
    private Joystick joystick;
    //플레이어 이동 속도
    public float moveSpeed = 5f;

    //플레이어의 리지드바디
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    void Start() {
        joystick = Joystick.instance;
        if(joystick == null) {
            Debug.LogError("Joystick 인스턴스를 찾을 수 없습니다.");
        }
        //플레이어 리지드바디 컴포넌트 가져오기
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }



    private void FixedUpdate() {
        Rotate();
        Move();
        //Debug.Log(joystick.Vertical);
        playerAnimator.SetFloat("Move", joystick.Direction.magnitude);

    }

    private void Move() {
        //조이스틱의 입력 값을 벡터로 받음
        Vector3 direction = new Vector3(joystick.Direction.x, 0 , joystick.Direction.y);
        //입력 값을 통한 플레이어 이동
        playerRigidbody.velocity = direction * moveSpeed;
       
    }

    private void Rotate() {
        //조이스틱의 방향 각도를 구해서 회전값을 구함.
        float turn = Mathf.Atan2(joystick.Direction.x, joystick.Direction.y) * Mathf.Rad2Deg;
        //조이스틱의 방향 입력 값이 있을 경우에만 회전
        if (joystick.Direction.x != 0 || joystick.Direction.y != 0) {
            transform.rotation = Quaternion.Euler(0, turn, 0);
        }
    }

}
