using UnityEngine;

[CreateAssetMenu(fileName = "ConditionForwardTarget", menuName = "Behavior/Conditions/ForwardTarget")]
public class ConditionForwardTargetSO : BehaviorConditionSO
{
    [Header("Raycast Settings")]
    [SerializeField] float distance = 10f; // 레이캐스트 거리
    public override BehaviorCondition CreateCondition()
    {
        return new ConditionForwardTarget(distance);
    }
}
public class ConditionForwardTarget : BehaviorCondition
{
    float distance;
    public ConditionForwardTarget(float distance)
    {
        this.distance = distance;
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