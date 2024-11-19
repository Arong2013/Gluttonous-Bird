using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionCanState : BehaviorCondition
{
    List<Type> types;
    public ConditionCanState(List<Type> types)
    {
        this.types = types;
    }
    public override BehaviorState Execute()
    {
        if (types.Any(x => x ==  character.currentBState.GetType()))
        {
            return BehaviorState.SUCCESS;
        }
        Debug.Log("아아");
        return BehaviorState.FAILURE;
    }
}