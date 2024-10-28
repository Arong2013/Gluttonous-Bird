using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public abstract class BaseActionNode
{
    protected Character character;

    public BaseActionNode(Character character)
    {
        this.character = character;
    }

    public abstract TurnState Execute();
}

public class ActionSequence : BaseActionNode
{
    private List<ActionPhase> actionPhases;
    public Dictionary<string, object> decisionContext = new Dictionary<string, object>();

    public ActionSequence(Character character, List<ActionPhase> actionPhases) : base(character)
    {
        this.actionPhases = actionPhases;
        foreach (var phase in actionPhases)
        {
            phase.SetParentSequence(this); // 각 ActionPhase에 부모 설정
        }
    }

    public void SetData(string key, object value)
    {
        decisionContext[key] = value;
    }

    public override TurnState Execute()
    {
        foreach (ActionPhase phase in actionPhases)
        {
            var result = phase.Execute();
            if (result == TurnState.FAILURE) return TurnState.FAILURE;
            if (result == TurnState.RUNNING) return TurnState.RUNNING;
        }
        return TurnState.SUCCESS;
    }
}

public class ActionPhase : BaseActionNode
{
    private ActionSequence parentSequence;
    private List<TurnAction> conditions;
    private TurnAction taskAction;
    private TurnState currentTurnState;

    public ActionPhase(Character character, List<TurnAction> conditions, TurnAction taskAction) : base(character)
    {
        this.conditions = conditions;
        this.taskAction = taskAction;
        this.currentTurnState = TurnState.SUCCESS;
    }

    public override TurnState Execute()
    {
        if (currentTurnState == TurnState.RUNNING)
            return currentTurnState = taskAction.Execute();

        bool allConditionsMet = conditions.TrueForAll(c => c.Execute() == TurnState.SUCCESS);
        return currentTurnState = allConditionsMet ? taskAction.Execute() : TurnState.FAILURE;
    }

    public void SetData(string key, object value) => parentSequence.SetData(key, value);

    public void SetParentSequence(ActionSequence sequence) => parentSequence = sequence;
}

public abstract class TurnAction : BaseActionNode
{
    protected ActionPhase actionPhase;

    public TurnAction(Character character, ActionPhase actionPhase) : base(character)
    {
        this.actionPhase = actionPhase;
    }
    public abstract override TurnState Execute();
}
