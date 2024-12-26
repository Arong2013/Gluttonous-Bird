using System.Collections;
using UnityEngine;

public abstract class AttackState : CharacterState
{
    Animator animator;
    AnimationCurve attackCurve;

    float time, progress;
    bool isHitStop;
    protected AttackState(CharacterMarcine character, Animator animator, AnimationCurve attackCurve) : base(character)
    {
       this.animator = animator;    
        this.attackCurve = attackCurve; 
    }
    public override void Enter()
    {
        base.Enter();
        isHitStop = false; 
    }
    public override void Execute()
    {
        base.Execute();
        if (isHitStop)
            ApplyCurveEffect();
    }
    public abstract void BtnUp();

    public override void Exit()
    {
        base.Exit();
        if(character is PlayerMarcine player)
            player.WeaponAttackEnd();   
    }
    public void PlayHitStop() => isHitStop = true;
    public void ApplyCurveEffect()
    {
        progress = Mathf.Clamp01(time / 1f);
        float speedMultiplier = attackCurve.Evaluate(progress);
        animator.speed = speedMultiplier;
        time += Time.deltaTime;

        Debug.Log($"Progress: {progress}, Speed Multiplier: {speedMultiplier}");
    }
}