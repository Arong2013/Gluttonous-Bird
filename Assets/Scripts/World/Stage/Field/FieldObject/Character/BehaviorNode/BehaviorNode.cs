using System;
using System.Collections.Generic;
using System.Linq;

public abstract class BehaviorNode
{
    protected Character character;

    public BehaviorNode(Character character)
    {
        this.character = character;
    }
    public abstract BehaviorState Execute();
}

public class BehaviorSequence : BehaviorNode
{
    private List<BehaviorPhase> actionPhases;
    public Dictionary<string, object> decisionContext = new Dictionary<string, object>();

    public BehaviorSequence(Character character, List<BehaviorPhase> actionPhases) : base(character)
    {
        this.actionPhases = actionPhases;
        foreach (var phase in actionPhases)
        {
            phase.SetParentSequence(this); // 각 BehaviorPhase에 부모 설정
        }
    }
    public void SetData(string key, object value)
    {
        decisionContext[key] = value;
    }

    public override BehaviorState Execute()
    {
        foreach (BehaviorPhase phase in actionPhases)
        {
            var result = phase.Execute();
            if (result ==BehaviorState.FAILURE) return BehaviorState.FAILURE;
            if (result == BehaviorState.RUNNING) return BehaviorState.RUNNING;
        }
        return BehaviorState.SUCCESS;
    }
}

public class BehaviorPhase : BehaviorNode
{
    private BehaviorSequence parentSequence;
    private List<BehaviorCondition> conditions;
    private BehaviorAction taskAction;
    private BehaviorState currentTurnState;

    public BehaviorPhase(Character character, List<BehaviorCondition> conditions, BehaviorAction taskAction) : base(character)
    {
        this.conditions = conditions;
        this.taskAction = taskAction;
        this.currentTurnState = BehaviorState.SUCCESS;
    }

    public override BehaviorState Execute()
    {
        if (currentTurnState == BehaviorState.RUNNING)
            return currentTurnState = taskAction.Execute();

        bool allConditionsMet = conditions.TrueForAll(c => c.Execute() == BehaviorState.SUCCESS);
        return currentTurnState = allConditionsMet ? taskAction.Execute() : BehaviorState.FAILURE;
    }

    public void SetData(string key, object value) => parentSequence.SetData(key, value);
    public void SetParentSequence(BehaviorSequence sequence) => parentSequence = sequence;
}

public abstract class BehaviorCondition : BehaviorNode
{
    protected BehaviorPhase actionPhase;
    public BehaviorCondition(Character character, BehaviorPhase actionPhase) : base(character)
    {
        this.actionPhase = actionPhase;
    }
    public abstract override BehaviorState Execute();
}
public abstract class BehaviorAction : BehaviorNode
{
    protected BehaviorPhase actionPhase;
    public BehaviorAction(Character character, BehaviorPhase actionPhase) : base(character)
    {
        this.actionPhase = actionPhase;
    }
    public abstract override BehaviorState Execute();
}
