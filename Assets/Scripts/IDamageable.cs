using UnityEngine;

//������ �޴� Ÿ�Ե� �������̽�
public interface IDamageable {
    //������ ���� �� �ִ� Ÿ�Ե��� ���
    //������, ���� ����, ���� ǥ���� ����
    void OnDamage(float damage, Vector3 hitpoint, Vector3 hitNormal);
}
