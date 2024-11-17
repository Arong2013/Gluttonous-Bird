using System;
using UnityEngine;

public class ConditionCanState : BehaviorCondition
{
    Type StateType;
    public ConditionCanState(Type StateType)
    {
        this.StateType = StateType;
    }
    public override BehaviorState Execute()
    {
        if (character.currentBState.GetType() == StateType)
        {
            return BehaviorState.SUCCESS;
        }   
        return BehaviorState.FAILURE;
    }
}