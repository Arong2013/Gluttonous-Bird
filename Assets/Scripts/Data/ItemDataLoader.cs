using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class ItemDataLoader
{
    private readonly string dbPath = $"{Application.streamingAssetsPath}/ItemData.db";
    private readonly Dictionary<int, Item> loadedItems = new Dictionary<int, Item>();
    private readonly Dictionary<string, Sprite> itemSprites = new Dictionary<string, Sprite>(); // 아이템 이름과 스프라이트 매핑

    private static ItemDataLoader instance = null;

    /// <summary>
    /// 싱글톤 인스턴스 반환
    /// </summary>
    public static ItemDataLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemDataLoader();
            }
            return instance;
        }
    }

    private ItemDataLoader()
    {
        LoadAllSprites();
        LoadAllItems();
    }

    /// <summary>
    /// 모든 테이블의 아이템 데이터를 로드
    /// </summary>
    private void LoadAllItems()
    {
        foreach (ItemTableName tableName in Enum.GetValues(typeof(ItemTableName)))
        {
            LoadItemsFromTable(tableName);
        }
    }
    private void LoadAllSprites()
    {
        // Resources 경로에서 모든 Texture2D를 로드
        Texture2D[] textures = Resources.LoadAll<Texture2D>("ItemSprites"); // 리소스 경로: Resources/ItemSprites/
        foreach (Texture2D texture in textures)
        {
            string itemName = texture.name;
            if (!itemSprites.ContainsKey(itemName))
            {
                // Texture2D로부터 Sprite 생성
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f) // Pivot 설정: (0.5, 0.5)는 중심
                );

                itemSprites[itemName] = sprite;
            }
        }
    }

    /// <summary>
    /// 특정 테이블의 데이터를 로드
    /// </summary>
    private void LoadItemsFromTable(ItemTableName tableName)
    {
        string queryTableName = $"{tableName}";
        string connectionPath = GetConnectionPath();

        using (IDbConnection dbConnection = new SqliteConnection(connectionPath))
        {
            try
            {
                dbConnection.Open();
                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    string query = $"SELECT * FROM {queryTableName}";
                    dbCommand.CommandText = query;

                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ItemData item = CreateItem(tableName, reader);
                            if (item != null && !loadedItems.ContainsKey(item.ID))
                            {
                                loadedItems[item.ID] = new Item(item, GetSpriteByName(item.Name));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load data from {queryTableName}: {ex.Message}");
            }
        }
    }
    /// <summary>
    /// 특정 테이블의 데이터로 아이템 객체 생성
    /// </summary>
    private ItemData CreateItem(ItemTableName tableName, IDataReader reader)
    {
        switch (tableName)
        {
            case ItemTableName.HammerItemData:
                return new HammerItemData
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Information = reader.GetString(2),
                    ATK = reader.GetInt32(3)
                    
                };
            case ItemTableName.ArmorItemData:
                return new ArmorItemData
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Information = reader.GetString(2),
                    DEF = reader.GetInt32(3)
                };
            case ItemTableName.RecoveryItemData:
                return LoadRecoveryItem(reader);
            default:
                Debug.LogWarning($"Unknown table name: {tableName}");
                return null;
        }
    }

    private RecoveryItemData LoadRecoveryItem(IDataReader reader)
    {
        RecoveryItemData recoveryItem = new RecoveryItemData
        {
            ID = reader.GetInt32(0),
            Name = reader.GetString(1),
            Information = reader.GetString(2),
            Amount = reader.GetInt32(3),
            MaxAmount = reader.GetInt32(4)
        };
        foreach (CharacterStatName stat in Enum.GetValues(typeof(CharacterStatName)))
        {
            float statValue = reader.GetFloat((int)stat + 5); // 스탯 컬럼은 3번째 이후부터 시작
            if (statValue > 0)
            {
                recoveryItem.SetRecoveryStat(stat, statValue);
            }
        }
        return recoveryItem;
    }

    /// <summary>
    /// DB 연결 경로 반환
    /// </summary>
    private string GetConnectionPath()
    {
        return $"URI=file:{dbPath.Replace("\\", "/")}";
    }

    /// <summary>
    /// ID로 아이템 데이터를 가져오기
    /// </summary>
    public Item GetItemByID(int itemID)
    {
        if (loadedItems.TryGetValue(itemID, out var itemData))
        {
            return itemData;
        }
        Debug.LogWarning($"Item with ID {itemID} not found!");
        return null;
    }

    public Sprite GetSpriteByName(string itemName)
    {
        if (itemSprites.TryGetValue(itemName, out var sprite))
        {
            return sprite;
        }
        Debug.LogWarning($"Sprite for item '{itemName}' not found!");
        return null;
    }

}
/// <summary>
/// 테이블 이름 열거형
/// </summary>
public enum ItemTableName
{
    ArmorItemData,
    HammerItemData,
    RecoveryItemData
}

/// <summary>
/// 기본 아이템 데이터 클래스
/// </summary>
