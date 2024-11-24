using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerMarcine : CharacterMarcine, ISubject
{
     Inventory Inventory;
     PlayerInputHandler inputHandler;
     PlayerIonsAndBar ionsAndBar;
     List<IObserver> observers = new List<IObserver>();
     public  WeaponBehavior WeaponBehavior { get; private set; }
    
    public bool CanComboBtn { get; protected set;} void EnableCombo() => CanComboBtn = true; void DisableCombo() => CanComboBtn = false;
    public IHarvestable CurrentHarvestable { get; private set; }
    public Action InteractionAction { get; private set; }

    public override void Init()
    {
        var data = CharacterDataManager.GetSingleton();
        characterData = data.GetCharacterData(1);

        // 핸들러 초기화
        inputHandler = FindObjectOfType<PlayerInputHandler>();
        inputHandler.Init(this);

        WeaponBehavior = FindObjectOfType<WeaponBehavior>();
        WeaponBehavior.Initialize(this);

        ionsAndBar = FindObjectOfType<PlayerIonsAndBar>();
        ionsAndBar.Initialize(this);


        currentBState = new IdleState(this, animator);
        CharacterMovementHandler = new PlayerMovementHandler(this);
        CharacterCombatHandler = new PlayerCombatHandler(this);
    }

    private void Update()
    {
        // 움직임 상태 업데이트
        if ((Mathf.Abs(currentDir.x) > 0.1f || Mathf.Abs(currentDir.y) > 0.1f) && currentBState.GetType() != typeof(MoveState))
        {
            CharacterAnimatorHandler.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, currentDir.magnitude);
        }

        currentBState?.Execute();
    }
    public void ToggleClimb()
    {
        bool canClimb = CharacterAnimatorHandler.GetAnimatorValue<CharacterAnimeBoolName, bool>(CharacterAnimeBoolName.CanClimb);
        CharacterAnimatorHandler.SetAnimatorValue(CharacterAnimeBoolName.CanClimb, !canClimb);
    }
    public void RegisterObserver(IObserver observer) => observers.Add(observer);

    public void UnregisterObserver(IObserver observer) => observers.Remove(observer);
    void WeaponAttackStart() => WeaponBehavior.ColliderSet(true); public void WeaponAttackEnd() => WeaponBehavior.ColliderSet(false);
    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.UpdateObserver();
        }
    }

    // 채집 로직
    public void HarvestObject()
    {
        if (CurrentHarvestable == null) return;

        var itemID = CurrentHarvestable.GetHarvestReward();
        var item = ItemDataLoader.Instance.GetItemByID(itemID);

        if (Inventory.AddItem(item))
        {
            Debug.Log($"Item {itemID} added to inventory.");
        }
        else
        {
            Debug.Log($"Failed to add item {itemID}. Inventory full.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {

            InteractionAction += () =>interactable.InteractEnter(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            InteractionAction = null;
        }
    }
}
public class Inventory
{
    private PlayerData playerData;
    private List<Item> items;

    public Inventory(List<Item> items, PlayerData playerData)
    {
        this.items = items;
        this.playerData = playerData;
    }

    // 아이템 추가
    public bool AddItem(Item item)
    {
        if (item.ItemData is ConsumableItemData consumable)
        {
            return AddConsumableItem(consumable);
        }

        if (items.Count >= playerData.InventoryMaxCount)
        {
            return false;
        }

        items.Add(item);
        return true;
    }

    private bool AddConsumableItem(ConsumableItemData consumable)
    {
        var matchingItems = items.FindAll(x => x.ItemData.ID == consumable.ID);

        foreach (var matchingItem in matchingItems)
        {
            int excess = AddAmountAndGetExcess(matchingItem.ItemData as ConsumableItemData, consumable.Amount);
            SetAmount(consumable, -excess);

            if (consumable.Amount <= 0)
            {
                return true;
            }
        }
        if (items.Count >= playerData.InventoryMaxCount)
        {
            return false;
        }
        items.Add(new Item(consumable,ItemDataLoader.Instance.GetSpriteByName(consumable.Name)));
        return true;
    }

    public int AddAmountAndGetExcess(ConsumableItemData consumable, int amount)
    {
        int newAmount = consumable.Amount + amount;
        SetAmount(consumable, newAmount);

        return Mathf.Max(0, newAmount - consumable.MaxAmount);
    }

    public void SetAmount(ConsumableItemData consumable, int amount)
    {
        consumable.Amount = Mathf.Clamp(amount, 0, consumable.MaxAmount);
    }

    public int SeparateItem(ConsumableItemData consumable, int amount)
    {
        if (consumable.Amount <= 1) return 0;

        int splitAmount = Mathf.Min(amount, consumable.Amount - 1);
        consumable.Amount -= splitAmount;

        return splitAmount;
    }
}
