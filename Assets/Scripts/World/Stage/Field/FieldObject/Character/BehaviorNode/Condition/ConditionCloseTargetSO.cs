using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ConditionParabolicMover", menuName = "Behavior/Conditions/ConditionParabolicMover")]
public class ConditionParabolicMoverSO : BehaviorConditionSO
{
    [SerializeField] private LayerMask obstacleLayer;


    public override BehaviorCondition CreateCondition()
    {
        return new ConditionParabolicMover(obstacleLayer);
    }
}

public class ConditionParabolicMover : BehaviorCondition
{
    LayerMask obstacleLayer;    
    public ConditionParabolicMover(LayerMask layerMask)
    {
        obstacleLayer = layerMask;  
    }

    public override BehaviorState Execute()
    {
        return BehaviorState.SUCCESS;
    }
}
