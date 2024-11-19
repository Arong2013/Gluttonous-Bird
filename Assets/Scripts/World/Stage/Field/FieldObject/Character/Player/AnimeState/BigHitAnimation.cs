using System;
using UnityEngine;

public class BigHitAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new BigHitState(character));
    }
}
public class BigHitState : CharacterState
{
    public BigHitState(CharacterMarcine character) : base(character) { }

    public override void Enter()
    {
        base.Enter();
        var dmg = character.currentDMG;
        character.SetDMG(0);
        foreach (CharacterAnimeBool boolName in Enum.GetValues(typeof(CharacterAnimeBool)))
        {
            character.SetAnimatorBool(boolName, false);
        }
        foreach (CharacterAnimeFloat floatName in Enum.GetValues(typeof(CharacterAnimeFloat)))
        {
            character.SetAnimatorFloat(floatName, 0);
        }
        foreach (CharacterAnimeIntName intName in Enum.GetValues(typeof(CharacterAnimeIntName)))
        {
            character.SetAnimatorInt(intName, 0);
        }
        if (character.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 backwardForce = -character.transform.forward * 10f;
            rb.AddForce(backwardForce, ForceMode.Impulse);
        }

    }
}
