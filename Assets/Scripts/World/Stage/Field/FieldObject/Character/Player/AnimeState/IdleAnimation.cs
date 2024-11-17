using System;
using UnityEngine;

public class IdleAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new IdleState(character));
    }
}
public class IdleState : CharacterState
{
    public IdleState(CharacterMarcine character) : base(character) { }

    public override void Enter()
    {
        base.Enter();
        foreach (CharacterAnimeBool boolName in Enum.GetValues(typeof(CharacterAnimeBool)))
        {
            character.SetAnimatorBool(boolName, false);
        }
    }
    public override void Execute()
    {

    }
}
