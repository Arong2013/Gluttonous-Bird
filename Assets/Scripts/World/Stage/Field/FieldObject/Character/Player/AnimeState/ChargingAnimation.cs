using UnityEngine;
public class ChargingAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new ChargingState(character));
    }
}
public class ChargingState : AttackState
{
    float ChargingCount;
    public ChargingState(CharacterMarcine character) : base(character)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        character.characterAnimatorHandler.SetAnimatorValue(CharacterAnimeBoolName.CanCharging, true);
        character.characterAnimatorHandler.SetAnimatorValue(CharacterAnimeFloatName.ChargingCount, 0);
        ChargingCount = 0;
    }
    public override void Execute()
    {
        base.Execute();
        ChargingCount += Time.deltaTime;
        character.characterAnimatorHandler.SetAnimatorValue(CharacterAnimeFloatName.ChargingCount, ChargingCount);
    }
    public override void BtnUp()
    {
        character.characterAnimatorHandler.SetAnimatorValue(CharacterAnimeBoolName.CanCharging, false);
    }
}