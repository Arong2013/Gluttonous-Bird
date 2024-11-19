using System;
using UnityEngine;

public class RoarAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new RoarState(character));
    }
}
public class RoarState : CharacterState
{
    public RoarState(CharacterMarcine character) : base(character) { }

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
