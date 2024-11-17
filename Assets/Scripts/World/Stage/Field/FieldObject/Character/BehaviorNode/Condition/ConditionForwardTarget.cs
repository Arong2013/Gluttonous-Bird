using UnityEngine;

public class ConditionForwardTarget : BehaviorCondition
{
    float distance;
    public ConditionForwardTarget(float Dis)
    {
        distance = Dis;
    }
    public override BehaviorState Execute()
    {
        RaycastHit hit;
        if (Physics.Raycast(character.transform.position, character.transform.forward, out hit, distance, character.EnemyLayer))
        {
            actionPhase.SetData("target", hit.transform);
            return BehaviorState.SUCCESS;
        }
        return BehaviorState.FAILURE;
    }
}