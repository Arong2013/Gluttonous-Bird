
using UnityEngine;
public class MonsterPartData
{
    public float hp;
    public float disDMG =100;
}
public class MonsterPart : MonoBehaviour, ICombatable
{
    public MonsterMarcine monsterMarcine { get; private set; }
    MonsterPartData monsterPartData;
    public void Init(MonsterMarcine monsterMarcine)
    {
        this.monsterMarcine = monsterMarcine;
        monsterPartData = new MonsterPartData();
        monsterPartData.hp = 100;
    }
    public void TakeDamage(float dmg, CharacterAnimeBool characterAnimeBool)
    {
        dmg = dmg * monsterPartData.disDMG * 0.01f;
        monsterPartData.hp -= dmg;
        Instantiate(ParticleResourceData.Instance.GetParticle("Blood"), transform.position, Quaternion.identity);
        if (monsterPartData.hp <= 0)
            monsterMarcine.SetAnimatorBool(CharacterAnimeBool.CanBigHit, true);
        monsterMarcine.TakeDamge(dmg);
    }
}