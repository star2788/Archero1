using UnityEngine;

//데미지 받는 타입들 인터페이스
public interface IDamageable {
    //데미지 입을 수 있는 타입들은 상속
    //데미지, 맞은 지점, 맞은 표면의 방향
    void OnDamage(float damage, Vector3 hitpoint, Vector3 hitNormal);
}
