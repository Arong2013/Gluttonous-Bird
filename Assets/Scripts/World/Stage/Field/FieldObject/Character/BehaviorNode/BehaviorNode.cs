using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class BehaviorSequence 
{
    private List<BehaviorPhase> actionPhases;
    public Dictionary<string, object> decisionContext = new Dictionary<string, object>();
    public CharacterMarcine character { get;protected set; }

    public BehaviorSequence(CharacterMarcine character,List<BehaviorPhase> actionPhases) 
    {
        this.actionPhases = actionPhases;
        this.character = character; 
        foreach (var phase in actionPhases)
        {
            phase.SetParentSequence(this); // 각 BehaviorPhase에 부모 설정
        }
    }
    public void SetData(string key, object value)
    {
        decisionContext[key] = value;
    }

    public BehaviorState Execute()
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

public class BehaviorPhase
{
    public CharacterMarcine character => parentSequence.character;
    private BehaviorSequence parentSequence;
    private List<BehaviorCondition> conditions;
    private BehaviorAction taskAction;
    private BehaviorState currentTurnState;

    public BehaviorPhase(List<BehaviorCondition> conditions, BehaviorAction taskAction)
    {
        this.conditions = conditions;
        this.taskAction = taskAction;
        this.currentTurnState = BehaviorState.SUCCESS;

        conditions.ForEach(condition => condition.SetParent(this));
        taskAction.SetParent(this);
    }
    public BehaviorState Execute()
    {
        if (currentTurnState == BehaviorState.RUNNING)
            return currentTurnState = taskAction.Execute();

        bool allConditionsMet = conditions.TrueForAll(c => c.Execute() == BehaviorState.SUCCESS);
        return currentTurnState = allConditionsMet ? taskAction.Execute() : BehaviorState.FAILURE;
    }

    public void SetData(string key, object value) => parentSequence.SetData(key, value);

    public object GetData(string key)
    {
        return parentSequence.decisionContext[key];
    }
    public void SetParentSequence(BehaviorSequence sequence) => parentSequence = sequence;
}

public abstract class BehaviorCondition
{
    protected CharacterMarcine character => actionPhase.character;
    protected BehaviorPhase actionPhase;
    public abstract BehaviorState Execute();
    public void SetParent(BehaviorPhase behaviorPhase) { this.actionPhase = behaviorPhase; }    
}
public abstract class BehaviorAction 
{
    protected CharacterMarcine character => actionPhase.character;
    protected BehaviorPhase actionPhase;
    public abstract BehaviorState Execute();
    public void SetParent(BehaviorPhase behaviorPhase) { this.actionPhase = behaviorPhase; }
}