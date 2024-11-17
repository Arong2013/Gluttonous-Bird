using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class MonsterMarcine : CharacterMarcine
{
    [SerializeField] List<MonsterPart> monsterParts;
    List<BehaviorSequence> behaviorSequences = new List<BehaviorSequence>(); 
    public float HP { get; set; }
    public override void Init()
    {
        var task = new ActionChessTarget();
        var con1 = new ConditionCanState(typeof(IdleState));
        var con2 = new ConditionForwardTarget(10f);
        var pase = new BehaviorPhase(new List<BehaviorCondition> { con1,con2} ,task);
        var seq = new BehaviorSequence(this, new List<BehaviorPhase> { pase });
        behaviorSequences.Add(seq);
    }

    public override void Move()
    {

        float speed = 10f * currentDir.magnitude; 
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
        for (var i =0; i< behaviorSequences.Count; i++)
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
        HP -= dmg;
        if(HP <= 0)
        {
            Dead();
            return;
        }
    }
    public void Dead()
    {
        Debug.Log("Dead");
    }
}