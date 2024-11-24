using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;



public enum CharacterStatName
{
    HP, MaxHP,
    SP, MaxSP,
    ATK,
    DEF,
    SPD
}


public enum CharacterAnimeBoolName
{
    CanRoll,
    CanCombo,
    CanCharging,
    CanDead,
    CanJump,
    CanClimb
}
public enum CharacterAnimeFloatName
{
    ChargingCount,
    SpeedCount,
}
public enum CharacterAnimeIntName
{
    AttackType,
    HitType,
    RoarType
}
public abstract class CharacterMarcine : FieldObject, ICombatable
{

    [SerializeField] float groundCheckDistance;
    [SerializeField] protected LayerMask layerMask;
  

    public LayerMask EnemyLayer => layerMask;
    public CharacterData characterData { get; protected set; }


    protected CapsuleCollider capsuleCollider;
    protected Animator animator;
    protected CharacterState currentBState;
    protected CharacterAnimatorHandler CharacterAnimatorHandler;
    protected CharacterMovementHandler CharacterMovementHandler;
    protected CharacterCombatHandler CharacterCombatHandler;

    public Vector2 currentDir { get; protected set; }
    public float currentDMG { get; protected set; }
    public bool isGround => IsGrounded();
    private void Awake()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        CharacterAnimatorHandler = new CharacterAnimatorHandler(animator);
    }
    private void Start()
    {
        Init();
    }
    public abstract void Init();
    public void Move() => CharacterMovementHandler.Move();
    public void Roll() => CharacterMovementHandler.Roll();
    public void TakeDamge(DamgeData damgeData) => CharacterCombatHandler.TakeDamage(damgeData);
    public void Climb() => CharacterMovementHandler.Climb();
    public void Jump()
    {
        if (isGround)
        {
            CharacterMovementHandler.Jump();
            CharacterAnimatorHandler.SetAnimatorValue(CharacterAnimeBoolName.CanJump, true);
        }

    }
    public void ChangePlayerState(CharacterState newState)
    {
        currentBState?.Exit();
        currentBState = newState;
        currentBState.Enter();
    }
    public Type GetCharacterStateType() => currentBState.GetType();
    public CharacterState GetState() => currentBState;
    public void SetDir(Vector2 dir) { currentDir = dir; }
    public void SetDMG(float dmg) { currentDMG = dmg; }
    public bool IsGrounded()
    {
        Vector3 checkOrigin = transform.position + Vector3.zero + Vector3.up * 0.1f;
        var bools = Physics.Raycast(checkOrigin, Vector3.down, groundCheckDistance, layerMask);
        return bools;
    }
    public void RollStart() => CharacterMovementHandler.RollAnimeEvent(capsuleCollider, true); public void RollEnd() => CharacterMovementHandler.RollAnimeEvent(capsuleCollider, false);
    public void SetAnimatorValue<T>(T type, object value) where T : Enum { CharacterAnimatorHandler.SetAnimatorValue(type, value);}
    public TResult GetAnimatorValue<T, TResult>(T type) where T : Enum { return CharacterAnimatorHandler.GetAnimatorValue<T,TResult>(type); }
}
