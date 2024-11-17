using UnityEngine;
using System;
using System.Collections;
public class Hammer : WeaponBehavior
{
    BoxCollider boxCollider;
    float chargingcount;
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
}
