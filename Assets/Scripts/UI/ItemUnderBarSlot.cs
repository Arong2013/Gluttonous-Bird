using UnityEngine;
using UnityEngine.UI;

public class ItemUnderBarSlot : MonoBehaviour
{
    ConsumableItemData ConsumableItemData;
    Item item;
    [SerializeField] Image icon;
    public void SetItem(Item item)
    {
        this.item = item;
        if (item.ItemData is ConsumableItemData consumable)
            ConsumableItemData = item.ItemData as ConsumableItemData;
        icon.sprite = item.icon;
    }
}