using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public abstract class Character : FieldObject, ITurnStateable
{
    Animator animator;
    public abstract TurnState CurrentTurnState { get; set; }
    public abstract TurnState ExecuteTurn();
    public TurnState Move(Vector3 nextPos)
    {
        float distanceToTarget = Vector3.Distance(Position, nextPos);
        Position = nextPos;
        if (distanceToTarget <= 0.1f)
        {
            animator.CrossFade("Idle", 0.2f);
            return TurnState.SUCCESS;
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            animator.Play("Walk");

        Vector3 direction = (nextPos - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, nextPos, 5f * Time.deltaTime);
        return TurnState.RUNNING;
    }
}