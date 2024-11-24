using UnityEngine;
using UnityEngine.TextCore.Text;
public class PlayerCombatHandler : CharacterCombatHandler
{
    PlayerMarcine PlayerMarcine;
    private Rigidbody rigidbody;
    public PlayerCombatHandler(CharacterMarcine character)
    {
        this.PlayerMarcine = character as PlayerMarcine;
        this.rigidbody = character.GetComponent<Rigidbody>();
    }
    public override void TakeDamage(DamgeData damgeData)
    {
        PlayerMarcine.characterData.UpdateBaseStat(CharacterStatName.HP, -damgeData.Dmg);
        PlayerMarcine.NotifyObservers();
        if (damgeData.DamgeAnimeType > 0)
        {
            PlayerMarcine.SetAnimatorValue(CharacterAnimeIntName.HitType, damgeData.DamgeAnimeType); 
        }
    }
}
