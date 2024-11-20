using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
   public int ID { get; set; }
    public string IconName { get; set; }
    public string Name { get; set; }
    public string Information { get; set; }
}

public class HammerItemData : ItemData
{
    public int ATK { get; set; }
}

public abstract class WeaponItemData : ItemData
{
    public string PrefabsName { get; set; }
}
public class ArmorItemData : ItemData
{
    public int DEF { get; set; }
}
public abstract class ConsumableItemData : ItemData
{
    public int Amount { get; set; }
    public int MaxAmount { get; set; }
}
public class RecoveryItemData : ConsumableItemData
{
    private Dictionary<CharacterStatName, float> recoveryStats = new Dictionary<CharacterStatName, float>();
    public void SetRecoveryStat(CharacterStatName statName, float amount)
    {
        if (recoveryStats.ContainsKey(statName))
        {
            recoveryStats[statName] = amount;
        }
        else
        {
            recoveryStats.Add(statName, amount);
        }
    }
    public float GetRecoveryStat(CharacterStatName statName)
    {
        return recoveryStats.ContainsKey(statName) ? recoveryStats[statName] : 0;
    }
    public Dictionary<CharacterStatName, float> GetAllRecoveryStats()
    {
        return new Dictionary<CharacterStatName, float>(recoveryStats);
    }
}
public class Item
{
   public Sprite icon { get; set; }
   public ItemData ItemData { get; set; }
    public Item(ItemData itemData,Sprite sprite)
    {
        ItemData = itemData;
        icon = sprite;  
    }
}
