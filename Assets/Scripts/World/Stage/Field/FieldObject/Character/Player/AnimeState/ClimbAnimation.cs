using UnityEngine;

public class ClimbAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new ClimbState(character));
    }
}

public class ClimbState : CharacterState
{
    private Rigidbody rigidbody;
    private float rayDistance = 0.5f; // Raycast 거리

    public ClimbState(CharacterMarcine character) : base(character) { }

    public override void Enter()
    {
        base.Enter();
        rigidbody = character.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.useGravity = false;
    }
    public override void Execute()
    {
        base.Execute();
        character.Climb();
    }
    public override void Exit()
    {
        base.Exit();
        rigidbody.useGravity = true;
        character.SetAnimatorValue(CharacterAnimeBoolName.CanJump, false);
    }
    private bool IsOnLadder()
    {
        return Physics.Raycast(character.transform.position, character.transform.forward, rayDistance, LayerMask.GetMask(ObjectLayerMask.Ladder.ToString()));
    }
}
