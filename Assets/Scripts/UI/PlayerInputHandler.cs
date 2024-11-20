using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMarcine playerMarcine;
    [SerializeField] Joystick joystick;
    [SerializeField] AttackBtn attackBtn;
    [SerializeField] Button roolBtn;
    [SerializeField] Button interactionBtn;
    public void Init(PlayerMarcine _playerMarcine)
    {
        playerMarcine = _playerMarcine;
        attackBtn.Init(playerMarcine);
        roolBtn.onClick.AddListener(() => { playerMarcine.characterAnimatorHandler.SetAnimatorValue(CharacterAnimeBoolName.CanRoll,true); });
        interactionBtn.onClick.AddListener(() => playerMarcine.interaction?.Invoke());
    }
    public void Update()
    {
        if (!playerMarcine)
            return;
        playerMarcine.SetDir(joystick.Direction);
    }
}
