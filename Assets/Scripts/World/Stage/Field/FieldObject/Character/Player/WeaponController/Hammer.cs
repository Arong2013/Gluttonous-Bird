using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Hammer : WeaponBehavior
{
    float chargingcount;
    List<MonsterMarcine> monsterMarcines = new List<MonsterMarcine>();
    public override void BtnDown()
    {
        player.SetAnimatorBool(CharacterAnimeBool.CanAttack,true);
    }
    public override void BtnUp()
    {
        if (player.currentBState is AttackState state)
        {
            state.BtnUp();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MonsterPart>(out MonsterPart combatable) && !other.GetComponent<PlayerMarcine>() && !monsterMarcines.Contains(combatable.monsterMarcine))
        {
          
            combatable.TakeDamage(weaponDMG * disdmg * 0.01f,0);
        }
    }
}
