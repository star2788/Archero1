using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/BowData", fileName = "Bow Data")]
public class BowData : ScriptableObject {
    
    public enum ElementalType {
        normal,
        fire,
        water,
        ice,
        lightning
    }

    public ElementalType elementalType; //활 속성

    public AudioClip shotClip; // 발사 소리

    public float damage = 25f; // 공격력 

    public float projectileSpeed = 1f; //투사체 속도

    public float timeBetFire = 0.3f; //발사 간격
 
    public float count; //화살 갯수

    public GameObject projectile; //화살 오브젝트

    public Color arrowLineColor; //속성에 따른 화살궤도 색상


}