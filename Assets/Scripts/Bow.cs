using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    public Transform fireTransform; // 화살이 발사될 위치

    public ParticleSystem arrowFireEffect; //화살 발사 이펙트

    private LineRenderer arrowLineRenderer; // 화살 궤적을 그리기 위한 렌더러

    private AudioSource bowAudioPlayer; // 활 소리 재생기

    public BowData bowData; // 활의 현재 데이터

    private float fireDistance = 50f; //사정거리

    private float lastFireTime; //총을 마지막으로 발사한 시점


    private void Awake() {

        bowAudioPlayer = GetComponent<AudioSource>();
        arrowLineRenderer = GetComponent<LineRenderer>();

        arrowLineRenderer.positionCount = 2;
        arrowLineRenderer.enabled = false;

        
    }

    private void OnEnable() {
        //마지막 활 쏜 시점 초기화
        lastFireTime = 0f;
    }


    //발사 시도
    public void Fire() {
      
        /*
        //마지막 활 쏜 시점에서 bowData.timeBetFire 시간이 지난 뒤
        if(Time.time >= lastFireTime + bowData.timeBetFire) {
            //마지막 활 쏜 시점 갱신
            lastFireTime = Time.time;
            //실제 발사 처리 실행
            Shot();
        }
       */


        Shot();

    }

    //실제 발사 처리
    private void Shot() {

        RaycastHit hit;

        Vector3 hitPosition = Vector3.zero;

        if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance)) {

            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target != null) {
                target.OnDamage(bowData.damage, hit.point, hit.normal);
            }

            hitPosition = hit.point;

        }
        else {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        Debug.Log(hitPosition);
        StartCoroutine(ShotEffect(hitPosition));

    }

    //발사 이펙트와 소리를 재생하고 화살 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition) {

        //활 쏘는 소리 효과 재생
        bowAudioPlayer.PlayOneShot(bowData.shotClip);

        //선 시작점은 활 쏘는 지점
        arrowLineRenderer.SetPosition(0, fireTransform.position);
        //선 끝점은 입력으로 들어온 충격지점
        arrowLineRenderer.SetPosition(1, hitPosition);
        //선 활성화
        arrowLineRenderer.enabled = true;

        //0.03초 쉬고
        yield return new WaitForSeconds(0.03f);

        //선 비활성화
        arrowLineRenderer.enabled = false;

    }


}
