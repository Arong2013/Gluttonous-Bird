using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class MonsterMarcine : CharacterMarcine
{
    [SerializeField] int baseMonsterID;
    [SerializeField] List<MonsterPart> monsterParts;
    [SerializeField] List<BehaviorSequenceSO> behaviorSequencesSO;


    List<BehaviorSequence> behaviorSequences = new List<BehaviorSequence>();
    public bool IsHarvest { get; set; }
    public object LastTarget { get; set; }
    public override void Init()
    {
        currentBState = new IdleState(this,animator);
        CharacterCombatHandler = new MonsterCombatHandler(this);
        CharacterMovementHandler = new MonsterMovementHandler(this);    

        behaviorSequencesSO.ForEach(sequence => behaviorSequences.Add(sequence.CreatBehaviorSequence(this)));
        var dataloder = MonsterDataLoader.GetSingleton();
        CharacterData monsterData = dataloder.GetMonsterBaseData(baseMonsterID);
        characterData = monsterData;

        foreach (var part in monsterParts)
        {
            var partData = dataloder.GetMonsterPartData(baseMonsterID, part.BasePartID);
            if (partData != null)
            {
                part.Init(this, partData); 
            }
        }
    }
    private void Update()
    {
        for (var i = 0; i < behaviorSequences.Count; i++)
        {
            var seq = behaviorSequences[i];
            if (seq.Execute() == BehaviorState.FAILURE)
                continue;
            else
                break;
        }
        currentBState?.Execute();
    }
    public void Dead()
    {
        CharacterAnimatorHandler.SetAnimatorValue(CharacterAnimeBoolName.CanDead, true);
    }
    public void SetHarvest(bool can) { IsHarvest = can; }
    public void SetLastTarget(object tag) => LastTarget = tag;

}