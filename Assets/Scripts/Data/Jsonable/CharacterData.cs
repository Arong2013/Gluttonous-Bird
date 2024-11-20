using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public readonly int ID;
    public readonly string Name;
    public int Level;
    public Vector3 position;

    [SerializeField]
    private SerializableDictionary<CharacterStatName, float> baseStats = new SerializableDictionary<CharacterStatName, float>();

    [NonSerialized]
    private Dictionary<CharacterStatName, Dictionary<object, float>> updatedStats = new Dictionary<CharacterStatName, Dictionary<object, float>>();

    public CharacterData(string name, int level)
    {
        Name = name;
        Level = level;

        foreach (CharacterStatName stat in Enum.GetValues(typeof(CharacterStatName)))
        {
            baseStats[stat] = 0;
        }
    }

    public CharacterData(int ID,string name)
    {
        this.ID = ID;   
        this.Name = name;   
        foreach (CharacterStatName stat in Enum.GetValues(typeof(CharacterStatName)))
        {
            baseStats[stat] = 0;
        }
    }

    public void SetBaseStat(CharacterStatName statName, float value)
    {
        if (baseStats.ContainsKey(statName))
        {
            baseStats[statName] = value;
            Debug.Log($"{statName}={value}");
        }
    }

    public void UpdateBaseStat(CharacterStatName statName, float value)
    {
        if (baseStats.ContainsKey(statName))
        {
            baseStats[statName] += value;
        }
    }

    public void UpdateStat(CharacterStatName statName, object source, float value)
    {
        if (!updatedStats.ContainsKey(statName))
        {
            updatedStats[statName] = new Dictionary<object, float>();
            updatedStats[statName].Add(source, 0);
        }
        updatedStats[statName][source] += value;
    }

    public void ChangeStat(CharacterStatName statName, object source, float value)
    {
        if (!updatedStats.ContainsKey(statName))
        {
            updatedStats[statName] = new Dictionary<object, float>();
        }
        updatedStats[statName][source] = value;
    }

    public float GetStat(CharacterStatName statName)
    {
        float baseValue = baseStats.ContainsKey(statName) ? baseStats[statName] : 0;

        if (updatedStats.ContainsKey(statName))
        {
            foreach (var bonus in updatedStats[statName].Values)
            {
                baseValue += bonus;
            }
        }

        return baseValue;
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(new CharacterJsonData
        {
            Name = this.Name,
            Level = this.Level,
            Position = this.position,
            BaseStats = this.baseStats
        }, true);
    }

    [Serializable]
    private class CharacterJsonData
    {
        public string Name;
        public int Level;
        public Vector3 Position;
        public SerializableDictionary<CharacterStatName, float> BaseStats;
    }
}
