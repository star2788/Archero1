using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    public Transform fireTransform; // ȭ���� �߻�� ��ġ

    public ParticleSystem arrowFireEffect; //ȭ�� �߻� ����Ʈ

    private LineRenderer arrowLineRenderer; // ȭ�� ������ �׸��� ���� ������

    private AudioSource bowAudioPlayer; // Ȱ �Ҹ� �����

    public BowData bowData; // Ȱ�� ���� ������

    private float fireDistance = 50f; //�����Ÿ�

    private float lastFireTime; //���� ���������� �߻��� ����


    private void Awake() {

        bowAudioPlayer = GetComponent<AudioSource>();
        arrowLineRenderer = GetComponent<LineRenderer>();

        arrowLineRenderer.positionCount = 2;
        arrowLineRenderer.enabled = false;

        
    }

    private void OnEnable() {
        //������ Ȱ �� ���� �ʱ�ȭ
        lastFireTime = 0f;
    }


    //�߻� �õ�
    public void Fire() {
      
        /*
        //������ Ȱ �� �������� bowData.timeBetFire �ð��� ���� ��
        if(Time.time >= lastFireTime + bowData.timeBetFire) {
            //������ Ȱ �� ���� ����
            lastFireTime = Time.time;
            //���� �߻� ó�� ����
            Shot();
        }
       */


        Shot();

    }

    //���� �߻� ó��
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

    //�߻� ����Ʈ�� �Ҹ��� ����ϰ� ȭ�� ������ �׸�
    private IEnumerator ShotEffect(Vector3 hitPosition) {

        //Ȱ ��� �Ҹ� ȿ�� ���
        bowAudioPlayer.PlayOneShot(bowData.shotClip);

        //�� �������� Ȱ ��� ����
        arrowLineRenderer.SetPosition(0, fireTransform.position);
        //�� ������ �Է����� ���� �������
        arrowLineRenderer.SetPosition(1, hitPosition);
        //�� Ȱ��ȭ
        arrowLineRenderer.enabled = true;

        //0.03�� ����
        yield return new WaitForSeconds(0.03f);

        //�� ��Ȱ��ȭ
        arrowLineRenderer.enabled = false;

    }


}
