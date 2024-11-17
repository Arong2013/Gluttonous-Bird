using UnityEngine;
public class ActionChessTarget : BehaviorAction
{
    Transform target; // 타겟 Transform
    float stopDistance = 4.0f; // 멈출 거리 (예: 1.0 미터)
    public override BehaviorState Execute()
    {
        if (target == null)
            this.target = actionPhase.GetData("target") as Transform;
        if (!character.GetAnimatorBool(CharacterAnimeBool.CanMove))
            character.SetAnimatorBool(CharacterAnimeBool.CanMove, true);

        float distanceToTarget = Vector3.Distance(character.transform.position, target.position);
        if (distanceToTarget <= stopDistance)
        {
            character.SetDir(new Vector2(0, 0));
            character.SetAnimatorBool(CharacterAnimeBool.CanMove, false);   
            return BehaviorState.SUCCESS; // 행동 완료 상태 반환
        }
        Vector3 direction = (target.position - character.transform.position).normalized;
        character.SetDir(new Vector2(direction.x,0));
        return BehaviorState.RUNNING; // 행동 실행 중 상태 반환
    }
}
