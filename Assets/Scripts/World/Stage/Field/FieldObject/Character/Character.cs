using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterData
{
    public string Name { get; private set; }
    public int Level { get; private set; }

    private Dictionary<CharacterStatName, float> baseStats = new Dictionary<CharacterStatName, float>();
    private Dictionary<CharacterStatName, Dictionary<object, float>> updatedStats = new Dictionary<CharacterStatName, Dictionary<object, float>>();

    public CharacterData(string name, int level)
    {
        Name = name;
        Level = level;

        foreach (CharacterStatName stat in Enum.GetValues(typeof(CharacterStatName)))
        {
            baseStats[stat] = 0;
        }
    }
    public void SetBaseStat(CharacterStatName statName, float value)
    {
        if (baseStats.ContainsKey(statName))
        {
            baseStats[statName] = value;
            Debug.Log($"{statName}={value}");
        }
    }
    public void UpdateStat(CharacterStatName statName, object source, float value)
    {
        if (!updatedStats.ContainsKey(statName))
        {
            updatedStats[statName] = new Dictionary<object, float>();
        }
        updatedStats[statName][source] = value;
    }

    public float GetStat(CharacterStatName statName)
    {
        float baseValue = baseStats.ContainsKey(statName) ? baseStats[statName] : 0;

        if (updatedStats.ContainsKey(statName))
        {
            foreach (var bonus in updatedStats[statName].Values)
            {
                baseValue += bonus;
            }
        }

        return baseValue;
    }
}

public enum CharacterStatName
{
    HP, MaxHP,
    SP, MaxSP,
    ATK,
    DEF,
    SPD
}


public enum CharacterAnimeBool
{
    CanMove,
    CanAttack,
    CanRoll,
    CanCombo,
    CanCharging
}
public enum CharacterAnimeFloat
{
    ChargingCount,
    SpeedCount,
}
public abstract class CharacterMarcine : FieldObject
{
    [SerializeField] protected LayerMask layerMask;


    protected Rigidbody rigidbody;
    protected Animator animator;



    public LayerMask EnemyLayer => layerMask;
    public CharacterData characterData { get; protected set; }
    public CharacterState currentBState { get; protected set; }
    public Vector2 currentDir { get; protected set; }
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
    public void SetAnimatorBool(CharacterAnimeBool boolname, bool isCan)
    {
        print(boolname.ToString());
        animator.SetBool(boolname.ToString(), isCan);
    }
    public void SetAnimatorFloat(CharacterAnimeFloat FloatType, float count)
    {
        animator.SetFloat(FloatType.ToString(), count);
    }

    public bool GetAnimatorBool(CharacterAnimeBool boolname)
    {
        return animator.GetBool(boolname.ToString());
    }
}
