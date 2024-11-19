using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
public class ActionAttack : BehaviorAction
{
    Transform target;
    int AttackType = 0;
    public ActionAttack(int attackType)
    {
        AttackType = attackType;    
    }
    public override BehaviorState Execute()
    {
        if (target == null)
            this.target = actionPhase.GetData("target") as Transform;
        if (character.currentBState.GetType() == typeof(NormalAttackState))
        {
            character.SetAnimatorInt(CharacterAnimeIntName.AttackType,0);
            return BehaviorState.RUNNING;
        }
        else
        {
            if (character.currentBState.GetType() == typeof(IdleState))
                return BehaviorState.SUCCESS;
            else
            {
                character.SetAnimatorInt(CharacterAnimeIntName.AttackType, AttackType);
                return BehaviorState.RUNNING;
            }
        }
    }
}
