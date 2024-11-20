using System;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorState
{
    SUCCESS,   
    RUNNING,   
    FAILURE    
}
public interface ICombatable
{
    void TakeDamage(float dmg,CharacterAnimeIntName characterAnimeBoo,int types);
}
public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void UnregisterObserver(IObserver observer);
    void NotifyObservers();
}
public interface IObserver
{
    void UpdateObserver();
}

public interface IHarvestable
{
    bool CanBeHarvested(); // 갈무리 가능한지 확인
    void StartHarvest();   // 갈무리 시작
    void EndHarvest();     // 갈무리 종료
    int GetHarvestReward(); // 갈무리로 얻는 보상 (예: 재료 수량)
}

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // Save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // Load the dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            throw new Exception($"Mismatched keys ({keys.Count}) and values ({values.Count}) during deserialization. Ensure all keys and values are serializable.");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            if (!this.ContainsKey(keys[i])) // Handle duplicate keys
            {
                this.Add(keys[i], values[i]);
            }
            else
            {
                Debug.LogWarning($"Duplicate key detected during deserialization: {keys[i]}");
            }
        }
    }
}
