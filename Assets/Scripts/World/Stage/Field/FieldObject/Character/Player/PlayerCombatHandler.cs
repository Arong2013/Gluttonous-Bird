﻿using UnityEngine;
using UnityEngine.TextCore.Text;
public class PlayerCombatHandler : CharacterCombatHandler
{
    PlayerMarcine character;
    private Rigidbody rigidbody;
    public PlayerCombatHandler(CharacterMarcine character, Rigidbody rigidbody)
    {
        this.character = character as PlayerMarcine;
        this.rigidbody = rigidbody;
    }
    public override void TakeDamage(DamgeData damgeData)
    {
        character.characterData.UpdateBaseStat(CharacterStatName.HP, -damgeData.Dmg);
        character.NotifyObservers();
        if (damgeData.DamgeAnimeType > 0)
        {
            character.SetAnimatorValue(CharacterAnimeIntName.HitType, damgeData.DamgeAnimeType); 
        }
    }
    public void Attack(ICombatable combatable, DamgeData damgeData)
    {
        combatable.TakeDamge(damgeData);
    }
}
