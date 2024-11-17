using UnityEngine;
using UnityEngine.EventSystems;

public class AttackBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private PlayerMarcine player;
    public void Init(PlayerMarcine playerMarcine)
    {
        player = playerMarcine; 
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        player?.weaponBehavior?.BtnDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player?.weaponBehavior?.BtnUp();
    }
}
