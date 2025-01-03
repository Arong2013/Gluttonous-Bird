﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour, IPlayerUesableUI
{
    private PlayerMarcine playerMarcine;
    [SerializeField] Joystick joystick;
    [SerializeField] AttackBtn attackBtn;
    [SerializeField] Button roolBtn;
    [SerializeField] Button JumpBtn;
    [SerializeField] Button interactionBtn;
    public void Initialize(PlayerMarcine playerMarcine)
    {
        this.playerMarcine = playerMarcine;
        attackBtn.Init(playerMarcine.WeaponBehavior);
        roolBtn.onClick.AddListener(() => playerMarcine.ToggleRoll());
        interactionBtn.onClick.AddListener(() => playerMarcine.InteractionBtnAction?.Invoke());
        JumpBtn.onClick.AddListener(() => playerMarcine.SetAnimatorValue(CharacterAnimeIntName.MovementType, (int)MovementType.Jump));
    }
    public void Update()
    {
        if (!playerMarcine)
            return;
        playerMarcine.SetDir(joystick.Direction);
    }
}
