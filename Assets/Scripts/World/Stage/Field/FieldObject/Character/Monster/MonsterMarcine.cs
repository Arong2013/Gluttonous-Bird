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
    public override void Init()
    {
        currentBState = new IdleState(this,animator);
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
    public override void Move()
    {
        float speed = characterData.GetStat(CharacterStatName.SPD) * currentDir.magnitude;
        Vector3 moveDirection = new Vector3(currentDir.x, 0, 0).normalized * speed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + moveDirection);
        if (currentDir.x != 0)
        {
            transform.rotation = Quaternion.Euler(0, currentDir.x > 0 ? 90 : -90, 0);
        }
    }
    public override void Roll()
    {

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
    public void TakeDamge(float dmg)
    {
        characterData.UpdateBaseStat(CharacterStatName.HP, -dmg);
        if (characterData.GetStat(CharacterStatName.HP) <= 0)
        {
            print(characterData.GetStat(CharacterStatName.HP));
            Dead();
            return;
        }
    }
    public void Dead()
    {
        characterAnimatorHandler.SetAnimatorValue(CharacterAnimeBoolName.CanDead, true);
    }
    public void MonsterRealDead()
    {

    }
    public void SetHarvest(bool can) { IsHarvest = can; }

}