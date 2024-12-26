using UnityEditor;
using UnityEngine;

public abstract class AttackAnimation : StateMachineBehaviour
{
    [SerializeField] protected float DisDamage;
}
public class NormalAttackAnimation : AttackAnimation
{
    private CharacterMarcine character;
    [SerializeField] AnimationCurve animationCurve;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new NormalAttackState(character, animator, animationCurve));
    }
}

public class NormalAttackState : AttackState
{
    public NormalAttackState(CharacterMarcine character, Animator animator, AnimationCurve attackCurve)
        : base(character, animator, attackCurve) { }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void BtnUp()
    {

    }
}
