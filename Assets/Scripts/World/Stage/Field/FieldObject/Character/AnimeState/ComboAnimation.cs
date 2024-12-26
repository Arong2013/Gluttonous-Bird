using UnityEngine;

public class ComboAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;
    [SerializeField] AnimationCurve animationCurve;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new ComboState(character, animator, animationCurve));
    }
}
public class ComboState : AttackState
{
    public ComboState(CharacterMarcine character, Animator animator, AnimationCurve attackCurve)
            : base(character, animator, attackCurve) { }
    public override void Enter()
    {
        base.Enter();
        character.SetAnimatorValue(CharacterAnimeBoolName.CanCombo, false);
    }
    public override void Execute()
    {
        base.Execute();
    }
    public override void BtnUp()
    {
        if (character is PlayerMarcine player && player.CanComboBtn)
            character.SetAnimatorValue(CharacterAnimeBoolName.CanCombo, true);
    }
    public override void Exit()
    {
        base.Exit();
    }
}