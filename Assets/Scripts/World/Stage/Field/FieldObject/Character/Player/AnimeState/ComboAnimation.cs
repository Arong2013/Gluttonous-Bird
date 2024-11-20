using UnityEngine;

public class ComboAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new ComboState(character));
    }
}
public class ComboState : AttackState
{
    public ComboState(CharacterMarcine character) : base(character)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        character.characterAnimatorHandler.SetAnimatorValue(CharacterAnimeBoolName.CanCombo, false);
    }
    public override void BtnUp()
    {
        if (character is PlayerMarcine player && player.CanComboBtn)
            character.characterAnimatorHandler.SetAnimatorValue(CharacterAnimeBoolName.CanCombo, true);
    }
}