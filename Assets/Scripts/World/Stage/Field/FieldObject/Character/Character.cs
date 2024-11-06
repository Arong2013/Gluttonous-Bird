using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public abstract class Character : FieldObject
{
    public CharacterState CurrentState { get; private set; }
    Rigidbody rigidbody;
    Animator animator;
    public bool CanPerformAction(CharacterState desiredAction)
    {
        return (int)desiredAction <= (int)CurrentState;
    }
    public void SetState(CharacterState newState)
    {
        if (CanPerformAction(newState))
        {
            CurrentState = newState;
        }
    }
    public virtual void Idle()
    {
        if(CanPerformAction(CharacterState.Idle))
        {
           if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.CrossFade("Idle", 0.1f);
            }
        }
    }
    public virtual void Walk()
    {

    }
    public static  bool SetCon(Character character)
    {
        return true;
    }
}