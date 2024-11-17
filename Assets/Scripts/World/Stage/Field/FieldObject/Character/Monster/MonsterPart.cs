
using UnityEngine;
public class MonsterPartData
{
    public float hp;
    public float disDMG =100;
}
public class MonsterPart : MonoBehaviour, ICombatable
{
    MonsterMarcine monsterMarcine;
    MonsterPartData monsterPartData;
    public void Init(MonsterMarcine monsterMarcine)
    {
        this.monsterMarcine = monsterMarcine;
        monsterPartData = new MonsterPartData();
        monsterPartData.hp = 100;
    }
    public void TakeDamage(float dmg)
    {
        dmg = dmg * monsterPartData.disDMG * 0.01f;
        monsterPartData.hp -= dmg;
        print($"몬스터데미지: {dmg}");
    }
}