using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMarcine playerMarcine;
    [SerializeField] Joystick joystick;
    [SerializeField] AttackBtn attackBtn;
    [SerializeField] Button roolBtn;
    [SerializeField] Button JumpBtn;
    [SerializeField] Button interactionBtn;
    public void Init(PlayerMarcine _playerMarcine)
    {
        playerMarcine = _playerMarcine;
        attackBtn.Init(playerMarcine.WeaponBehavior);
        roolBtn.onClick.AddListener(() => { playerMarcine.SetAnimatorValue(CharacterAnimeBoolName.CanRoll,true); });
        interactionBtn.onClick.AddListener(() => playerMarcine.InteractionAction?.Invoke());
        JumpBtn.onClick.AddListener(() => playerMarcine.Jump());
       
    }
    public void Update()
    {
        if (!playerMarcine)
            return;
        playerMarcine.SetDir(joystick.Direction);
    }
}
