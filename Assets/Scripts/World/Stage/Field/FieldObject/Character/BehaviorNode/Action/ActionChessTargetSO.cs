using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(fileName = "ActionChessTarget", menuName = "Behavior/Actions/ChessTarget")]
public class ActionChessTargetSO : BehaviorActionSO
{
    [SerializeField] float stopDistance = 4.0f; // 멈출 거리

    public override BehaviorAction CreateAction()
    {
        return new ActionChessTarget(stopDistance);
    }
}

public class ActionChessTarget : BehaviorAction
{
    Transform target;
    float stopDistance;
    NavMeshAgent agent;

    public ActionChessTarget(float stopDistance)
    {
        this.stopDistance = stopDistance;
    }
    public override BehaviorState Execute()
    {
        if (agent == null)
        {
            agent = character.transform.GetComponent<NavMeshAgent>();
            Debug.Log("에이전트 셋팅");
        }
        this.target = actionPhase.GetData("target") as Transform;
        agent.SetDestination(target.position);
        if (agent.hasPath)
        {
            character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, 0f);
            return BehaviorState.SUCCESS; // 행동 완료 상태 반환
        }
        else
        {
            character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, 0.1f);
            return BehaviorState.RUNNING; // 행동 실행 중 상태 반환
        }
    }
}
