using UnityEngine;
using UnityEngine.TextCore.Text;
public class MonsterCombatHandler : CharacterCombatHandler
{
    MonsterMarcine monsterMarcine;
    private Rigidbody rigidbody;
    public MonsterCombatHandler(CharacterMarcine character)
    {
        this.monsterMarcine = character as MonsterMarcine;
        this.rigidbody = character.GetComponent<Rigidbody>();
    }
    public override void TakeDamage(DamgeData damgeData)
    {
        monsterMarcine.characterData.UpdateBaseStat(CharacterStatName.HP, -damgeData.Dmg);
        monsterMarcine.SetLastTarget(damgeData.target);
        if(monsterMarcine.characterData.GetStat(CharacterStatName.HP) <= 0)
        {
            monsterMarcine.Dead();
            return;
        }
        if (damgeData.DamgeAnimeType > 0)
        {
            monsterMarcine.SetAnimatorValue(CharacterAnimeIntName.HitType, damgeData.DamgeAnimeType);
        }
    }
}
