﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;



public class PlayerMarcine : CharacterMarcine, ISubject
{
    PlayerData playerData = new PlayerData();
    List<IObserver> observers = new List<IObserver>();
    PlayerQuestHandler questHandler;
    public Action InteractionBtnAction { get; private set; }
    public WeaponBehavior WeaponBehavior { get; private set; }
    public Inventory inventory { get; private set; }
    public bool CanComboBtn { get; protected set; }
    void EnableCombo() => CanComboBtn = true; void DisableCombo() => CanComboBtn = false;
    public bool CanWalk => (Mathf.Abs(currentDir.x) > 0.1f || Mathf.Abs(currentDir.y) > 0.1f) && currentBState.GetType() != typeof(MoveState);
    public override void Init()
    {
        currentBState = new IdleState(this, animator);
        characterData = CharacterData.CreatPlayerData();
        inventory = new Inventory(playerData);

        var TestItem = ItemDataLoader.Instance.GetItemDataByID(1).CreatItem();
        var TestItem2 = ItemDataLoader.Instance.GetItemDataByID(1).CreatItem();
        inventory.AddItem(TestItem); inventory.AddItem(TestItem2);

        WeaponBehavior = FindObjectOfType<WeaponBehavior>();
        WeaponBehavior.Initialize(this);
        LinkUi();
        SetHandler();

        TakeDamge(new DamgeData(20, 0, this));
        NotifyObservers();

    }

    private void Update()
    {
        UpdataMovement();
        currentBState?.Execute();
    }
    void SetHandler()
    {
        CharacterMovementHandler = new PlayerMovementHandler(this, rigidbody);
        CharacterCombatHandler = new PlayerCombatHandler(this, rigidbody);
        questHandler = new PlayerQuestHandler(playerData);
        characterInteractionHandler = new PlayerInteractionHandler(this, rigidbody);
    }
    void LinkUi() => Utils.SetPlayerMarcineOnUI().ForEach(x => x.Initialize(this));
    void UpdataMovement()
    {
        if (IsFalling())
        {
            SetAnimatorValue(CharacterAnimeIntName.MovementType, (int)MovementType.Falling);
            return;
        }
        if (CanWalk)
            SetAnimatorValue(CharacterAnimeIntName.MovementType, (int)MovementType.Walk);
    }
    public void ToggleClimb()
    {
        int canClimb = (CharacterAnimatorHandler.GetAnimatorValue<CharacterAnimeIntName, int>(CharacterAnimeIntName.InteractionType) != (int)InteractionType.Climb) ? (int)InteractionType.Climb : 0;
        SetAnimatorValue(CharacterAnimeIntName.InteractionType, canClimb);
    }
    public void ToggleRoll()
    {
        if (characterData.GetStat(CharacterStatName.SP) > characterData.GetStat(CharacterStatName.RollSP))
        {
            SetAnimatorValue(CharacterAnimeIntName.MovementType, (int)MovementType.Roll);
        }
    }
    void WeaponAttackStart() => WeaponBehavior.ColliderSet(true); public void WeaponAttackEnd() => WeaponBehavior.ColliderSet(false);
    public void SetQuest(QuestData questData) => questHandler.SetQuest(questData);

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            InteractionBtnAction = () => interactable.InteractEnter(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            InteractionBtnAction = null;
        }
    }
}
public class Inventory
{
    private PlayerData playerData;
    private List<Item> playerItems;
    public List<Item> items
    {
        get
        {
            playerItems.RemoveAll(item => item is CountableItem countable && countable.Amount <= 0);
            return playerItems;
        }
        private set { }
    }

    public Inventory(PlayerData playerData)
    {
        this.playerItems = playerData.Items;
        this.playerData = playerData;
    }

    public Item GetItem(int index)
    {
        if (index >= 0 && index < items.Count) // 또는 items.Length, 타입에 따라
        {
            return items[index];
        }
        return null;
    }

    public bool AddItem(Item item)
    {
        if (item is CountableItem consumable)
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

    private bool AddConsumableItem(CountableItem consumable)
    {
        var matchingItems = items.FindAll(x => x.ID == consumable.ID);

        foreach (var matchingItem in matchingItems)
        {
            int excess = AddAmountAndGetExcess(matchingItem as CountableItem, consumable.Amount);
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
        items.Add(consumable);
        return true;
    }

    public int AddAmountAndGetExcess(CountableItem consumable, int amount)
    {
        int newAmount = consumable.Amount + amount;
        SetAmount(consumable, newAmount);

        return Mathf.Max(0, newAmount - consumable.MaxAmount);
    }

    public void SetAmount(CountableItem consumable, int amount)
    {
        consumable.Amount = Mathf.Clamp(amount, 0, consumable.MaxAmount);
    }
    public int SeparateItem(CountableItem consumable, int amount)
    {
        if (consumable.Amount <= 1) return 0;

        int splitAmount = Mathf.Min(amount, consumable.Amount - 1);
        consumable.Amount -= splitAmount;

        return splitAmount;
    }
}
