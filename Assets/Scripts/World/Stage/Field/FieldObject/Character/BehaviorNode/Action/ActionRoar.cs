using UnityEngine;
using UnityEngine.Rendering;
public class ActionRoar : BehaviorAction
{
    Transform target;
    bool canRoar;
    public override BehaviorState Execute()
    {
        if (target == null)
            this.target = actionPhase.GetData("target") as Transform; 
        if(character.currentBState.GetType() == typeof(RoarState))
        {
            canRoar = true;
            character.SetAnimatorBool(CharacterAnimeBool.CanRoar, false);
            return BehaviorState.RUNNING;
        }
        else
        {
            if (canRoar)
                return BehaviorState.SUCCESS;
            else
            {
                character.SetAnimatorBool(CharacterAnimeBool.CanRoar, true);
                return BehaviorState.RUNNING;
            }              
        }
    }
}
