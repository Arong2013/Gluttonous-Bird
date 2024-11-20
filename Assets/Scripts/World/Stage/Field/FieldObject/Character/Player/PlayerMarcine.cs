using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;
using static UnityEditor.Progress;
using System.Linq;
using Unity.VisualScripting;


public class PlayerMarcine : CharacterMarcine, ISubject, ICombatable
{
    PlayerInputHandler inputHandler { get; set; }
    public WeaponBehavior weaponBehavior { get; private set; }
    public Inventory inventory { get; private set; }
    public bool CanComboBtn { get; private set; }  public void EnableComboBtn() => CanComboBtn = true; public void DisableComboBtn() => CanComboBtn = false;
     public IHarvestable iHarvestable { get; private set; }

    List<IObserver> observers = new List<IObserver>();
    public Action interaction { get; private set; }
    public override void Init()
    {
        var data = CharacterDataManager.GetSingleton();
        characterData = data.GetCharacterData(1);

        inputHandler = FindObjectOfType<PlayerInputHandler>(); inputHandler.Init(this);
        weaponBehavior = FindObjectOfType<WeaponBehavior>(); weaponBehavior.Initialize(this);
        PlayerIonsAndBar playerIonsAndBar = FindObjectOfType<PlayerIonsAndBar>(); playerIonsAndBar.Initialize(this);

        currentBState = new IdleState(this, animator);        
    }
    private void Update()
    {
        if(Mathf.Abs(currentDir.x) > 0.1f || Mathf.Abs(currentDir.y) > 0.1f && currentBState.GetType() != typeof(MoveState))
        {
            characterAnimatorHandler.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, currentDir.magnitude); 
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
    public void TakeDamage(float dmg, CharacterAnimeIntName characterAnimeBool, int types)
    {
        if (types > 0)
        {
            currentDMG = dmg;   
            characterAnimatorHandler.SetAnimatorValue(characterAnimeBool,types);
            characterData.UpdateStat(CharacterStatName.HP, this, -dmg);
            NotifyObservers();
        }
        else
        {

        }
    }
    public void HarvestObj()
    {
       var ItemID = iHarvestable.GetHarvestReward();
        var item = ItemDataLoader.Instance.GetItemByID(ItemID); 
        if(inventory.AddItem(item))
        {

        }
        else
        { 
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<IHarvestable>(out IHarvestable monsterMarcine) && monsterMarcine.CanBeHarvested())
        {
            iHarvestable = monsterMarcine;
            interaction = null;
            interaction += HarvestObj;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<IHarvestable>(out IHarvestable monsterMarcine))
        {
            iHarvestable = null;
            interaction = null;
        }
    }
}
public class Inventory
{
    PlayerData playerData;  
    List<Item> items;
    public Inventory(List<Item> items, PlayerData playerData)
    {
        this.items = items;
        this.playerData = playerData;   
    }
    public bool AddItem(Item _item)
    {
        if (_item.ItemData is ConsumableItemData countItem)
        {
            var matchingItems = items.FindAll(x => x.ItemData.ID == countItem.ID);
            if (matchingItems.Count > 0)
            {
                foreach (var matchingItem in matchingItems)
                {
                    var less =  AddAmountAndGetExcess(matchingItem.ItemData as ConsumableItemData, countItem.Amount);
                   SetAmount(countItem, -less);
                    if (countItem.Amount <= 0)
                        return true;
                }
            }
        }
        if (items.Count >= playerData.InventoryMaxCount)
            return false;
        items.Add(_item);
        return true;
    }
    public int AddAmountAndGetExcess(ConsumableItemData consumableItemData,int amount)
    {
        int nextAmount = consumableItemData.Amount + amount;
        SetAmount(consumableItemData,nextAmount);
        return (nextAmount > consumableItemData.MaxAmount) ? (nextAmount - consumableItemData.MaxAmount) : 0;
    }
    public int Seperate(ConsumableItemData consumableItemData,int amount)
    {
        if (consumableItemData.Amount <= 1) return 0;

        if (amount > consumableItemData.Amount - 1)
            amount = consumableItemData.Amount - 1;

        consumableItemData.Amount -= amount;
        return amount;
    }
    public void SetAmount(ConsumableItemData consumableItemData,int amount)
    {
        consumableItemData.Amount = Mathf.Clamp(amount, 0, consumableItemData.MaxAmount);
    }

    
}