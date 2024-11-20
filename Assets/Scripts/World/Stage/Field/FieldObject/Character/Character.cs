using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



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
public abstract class CharacterMarcine : FieldObject
{
    [SerializeField] protected LayerMask layerMask;


    protected Rigidbody rigidbody;
    protected Animator animator;



    public LayerMask EnemyLayer => layerMask;
    public CharacterData characterData { get; protected set; }
    public CharacterState currentBState { get; protected set; }
    public CharacterAnimatorHandler characterAnimatorHandler { get; protected set; }    


    public Vector2 currentDir { get; protected set; }
    public float currentDMG { get; protected set; }
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        characterAnimatorHandler = new CharacterAnimatorHandler(animator);
    }
    private void Start()
    {
        Init(); 
    }
    public abstract void Init();

    public abstract void Move();

    public abstract void Roll();
    public void ChangePlayerState(CharacterState newState)
    {
        currentBState?.Exit();
        currentBState = newState;
        currentBState.Enter();
    }
    public void SetDir(Vector2 dir) { currentDir = dir; }
    public void SetDMG(float dmg) { currentDMG = dmg;}
}
