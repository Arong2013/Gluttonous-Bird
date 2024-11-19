using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

public class PlayerMarcine : CharacterMarcine, ISubject, ICombatable
{
    PlayerInputHandler inputHandler { get; set; }
    public WeaponBehavior weaponBehavior { get; private set; }
    public bool CanComboBtn { get; private set; }  public void EnableComboBtn() => CanComboBtn = true; public void DisableComboBtn() => CanComboBtn = false;
    List<IObserver> observers = new List<IObserver>();

    public override void Init()
    {

        var data = CharacterDataManager.GetSingleton();
        characterData = data.GetCharacterData(1);


        inputHandler = FindObjectOfType<PlayerInputHandler>(); inputHandler.Init(this);
        weaponBehavior = FindObjectOfType<WeaponBehavior>(); weaponBehavior.Initialize(this);
        PlayerIonsAndBar playerIonsAndBar = FindObjectOfType<PlayerIonsAndBar>(); playerIonsAndBar.Initialize(this);    

        currentBState = new IdleState(this);
    }
    private void Update()
    {
        if(Mathf.Abs(currentDir.x) > 0.1f || Mathf.Abs(currentDir.y) > 0.1f)
        {
            SetAnimatorBool(CharacterAnimeBool.CanMove,true);   
        }
        currentBState?.Execute();
    }
    public override void Move()
    {
        float speed = characterData.GetStat(CharacterStatName.SPD) * currentDir.magnitude;
        Vector3 moveDirection = new Vector3(currentDir.x, 0, 0).normalized * speed * Time.deltaTime;  print(speed);
        rigidbody.MovePosition(transform.position + moveDirection);
        if (currentDir.x != 0)
        {
            transform.rotation = Quaternion.Euler(0, currentDir.x > 0 ? 90 : -90, 0);
        }
    }
    public override void Roll()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        Vector3 backwardForce = transform.forward * 5f;
        rigidbody.AddForce(backwardForce, ForceMode.Impulse);
    }
    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void UnregisterObserver(IObserver observer)
    {
        observers.Remove(observer);
    }
    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.UpdateObserver();
        }
    }
    public void TakeDamage(float dmg , CharacterAnimeBool characterAnimeBool)
    {
        
        if (characterAnimeBool == CharacterAnimeBool.CanBigHit || characterAnimeBool == CharacterAnimeBool.CanHit)
        {
            currentDMG = dmg;   
            SetAnimatorBool(characterAnimeBool, true);
            characterData.UpdateStat(CharacterStatName.HP, this, -dmg);
            NotifyObservers();
        }
        else
        {

        }
    }
}