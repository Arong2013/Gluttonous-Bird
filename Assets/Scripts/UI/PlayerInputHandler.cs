using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMarcine playerMarcine;
    [SerializeField] Joystick joystick;
    [SerializeField] AttackBtn attackBtn;
    [SerializeField] Button roolBtn;
    public void Init(PlayerMarcine _playerMarcine)
    {
        playerMarcine = _playerMarcine;
        attackBtn.Init(playerMarcine);
        roolBtn.onClick.AddListener(() => { playerMarcine.SetAnimatorBool(CharacterAnimeBool.CanRoll,true); });
    }
    public void Update()
    {
        if (!playerMarcine)
            return;
        playerMarcine.SetDir(joystick.Direction);
    }
}
