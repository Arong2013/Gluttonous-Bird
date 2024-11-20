using UnityEngine;


public class MonsterPart : MonoBehaviour, ICombatable, IHarvestable
{
    [SerializeField] public int BasePartID;
    public MonsterMarcine monsterMarcine { get; private set; }
    MonsterPartData monsterPartData;

    public void Init(MonsterMarcine monsterMarcine,MonsterPartData  monsterPartData)
    {
        this.monsterMarcine = monsterMarcine;
        this.monsterPartData = monsterPartData;
    }
    public void TakeDamage(float dmg, CharacterAnimeIntName characterAnimeBool,int types)
    {
        dmg = dmg * monsterPartData.DisDMG * 0.01f;
        monsterPartData.HP -= dmg;
        Instantiate(ParticleResourceData.Instance.GetParticle("Blood"), transform.position, Quaternion.identity);
        if (monsterPartData.HP <= 0)
            monsterMarcine.characterAnimatorHandler.SetAnimatorValue(CharacterAnimeIntName.HitType,1);
        monsterMarcine.TakeDamge(dmg);
    }
    public bool CanBeHarvested()
    {
        return monsterMarcine.IsHarvest;
    }
    public void StartHarvest()
    {
        
    }
    public void EndHarvest()
    {
        monsterMarcine.MonsterRealDead();
    }
    public int GetHarvestReward()
    {
        return Utils.GetRandomItemFromDropTable(monsterPartData.DropItems);
    }
}