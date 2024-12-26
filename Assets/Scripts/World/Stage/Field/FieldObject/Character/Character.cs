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
    SPD,
    RollSP,
}


public enum CharacterAnimeBoolName
{
    CanCombo,
    CanCharging,
    CanDead,
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
    RoarType,
    MovementType,
    InteractionType
}
public enum MovementType
{
    Walk = 1,
    Jump = 2,
    Roll = 3,
    Falling = 4,
}

public enum InteractionType
{
    Climb = 1,
    Throw = 2
}

public abstract class CharacterMarcine : FieldObject, ICombatable,ISubject
{
    [SerializeField] protected Rigidbody rigidbody;
    [SerializeField] float groundCheckDistance;
    [SerializeField] protected LayerMask layerMask;



    protected List<IObserver> observers = new List<IObserver>();
    protected CapsuleCollider capsuleCollider;
    protected Animator animator;
    protected CharacterState currentBState;
    protected CharacterAnimatorHandler CharacterAnimatorHandler;
    protected CharacterMovementHandler CharacterMovementHandler;
    protected CharacterCombatHandler CharacterCombatHandler;
    protected CharacterInteractionHandler characterInteractionHandler;


    public LayerMask EnemyLayer => layerMask;
    public CharacterData characterData { get; protected set; }
    public Vector2 currentDir { get; protected set; }
    public float currentDMGPer { get; set; }
    public float currentDMG => characterData.GetStat(CharacterStatName.ATK) * currentDMG * 0.01f;
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
    public void Roll() { CharacterMovementHandler.Roll();NotifyObservers();}
    public void TakeDamge(DamgeData damgeData) => CharacterCombatHandler.TakeDamage(damgeData);
    public void Climb() => characterInteractionHandler.Climb();
    public void ThrowObj() => characterInteractionHandler.ThrowOBJ();
    public void Jump() => CharacterMovementHandler.Jump();
    public void ChangePlayerState(CharacterState newState)
    {
        currentBState?.Exit();
        currentBState = newState;
        currentBState.Enter();
    }
    public Type GetCharacterStateType() => currentBState.GetType();
    public CharacterState GetState() => currentBState;
    public void SetDir(Vector2 dir) { currentDir = dir; }
    public void SetDMGPer(float dmg) { currentDMGPer = dmg; }
    public void SetInteraction(InteractionType interactionType, params object[] objects)
    {
        characterInteractionHandler.SetInteraction(interactionType, objects);
        SetAnimatorValue(CharacterAnimeIntName.InteractionType, (int)interactionType);
    }
    public bool IsGrounded()
    {
        Vector3 checkOrigin = transform.position + Vector3.zero + Vector3.up * 0.1f;
        var bools = Physics.Raycast(checkOrigin, Vector3.down, groundCheckDistance, layerMask);
        return bools;
    }
    public bool IsFalling() => rigidbody.velocity.y < -3 && !isGround && animator.applyRootMotion == false;
    public void RollStart() => CharacterMovementHandler.RollAnimeEvent(capsuleCollider, true); public void RollEnd() => CharacterMovementHandler.RollAnimeEvent(capsuleCollider, false);
    public void SetAnimatorValue<T>(T type, object value) where T : Enum { CharacterAnimatorHandler.SetAnimatorValue(type, value);}
    public TResult GetAnimatorValue<T, TResult>(T type) where T : Enum { return CharacterAnimatorHandler.GetAnimatorValue<T,TResult>(type); }
    public void RegisterObserver(IObserver observer) => observers.Add(observer); public void UnregisterObserver(IObserver observer) => observers.Remove(observer);
    public void NotifyObservers() => observers.ForEach(x => x.UpdateObserver());
}
