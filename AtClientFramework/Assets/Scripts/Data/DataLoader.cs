using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Protocol;
using System.Linq;
using System;

public class DataLoader : SingletonMonoBehaviour<DataLoader>
{
    public Dictionary<EPlayerType, JobData> JobDataTable { get; private set; } = new Dictionary<EPlayerType, JobData>();
    public Dictionary<int, ItemData> ItemDataTable { get; private set; } = new Dictionary<int, ItemData>();

    public override void Initialize()
    {
        LoadJobData();
        LoadItemData();
    }

    private T ParseProtocolEnum<T>(JToken token) where T : struct, Enum
    {
        string rawValue = token["Value"].ToString();
        string enumString = rawValue.Split("::").Last().Replace("_", "");
        return Enum.Parse<T>(enumString, true); // true: 대소문자 무시
    }

    private void LoadJobData()
    {
        TextAsset jobJson = Resources.Load<TextAsset>("JsonData/ClassJson");
        JObject root = JObject.Parse(jobJson.text);

        foreach (var pair in root)
        {
            if (pair.Key.StartsWith("PLAYER_TYPE_"))
            {
                JObject jobJObject = (JObject)pair.Value;

                JobData jobData = new JobData
                {
                    jobType = ParseProtocolEnum<EPlayerType>(jobJObject["Id"]),
                    HP = (int)jobJObject["HP"]["Value"],
                    MP = (int)jobJObject["MP"]["Value"],
                    Damage = (int)jobJObject["Damage"]["Value"],
                    MagicDamage = (int)jobJObject["MagicDamage"]["Value"]
                };

                JobDataTable[jobData.jobType] = jobData;
            }
        }

        Debug.Log($"Job 데이터 {JobDataTable.Count}개 로드 완료.");
    }

    private void LoadItemData()
    {
        TextAsset itemJson = Resources.Load<TextAsset>("JsonData/ItemJson");
        JObject root = JObject.Parse(itemJson.text);

        foreach (var pair in root)
        {
            if (int.TryParse(pair.Key, out int itemId))
            {
                JObject itemJObject = (JObject)pair.Value;

                ItemData itemData = new ItemData
                {
                    Id = (int)itemJObject["Id"]["Value"],
                    Name = (string)itemJObject["Name"]["Value"],
                    jobType = ParseProtocolEnum<EPlayerType>(itemJObject["ClassType"]),
                    EquipSlotType = ParseProtocolEnum<EEquipSlotType>(itemJObject["EquipSlotType"]),
                    Stat = ParseProtocolEnum<EStat>(itemJObject["Stat"]),
                    StatParam = (int)itemJObject["StatParam"]["Value"]
                };
                ItemDataTable[itemData.Id] = itemData;
            }
        }

        Debug.Log($"Item 데이터 {ItemDataTable.Count}개 로드 완료.");
    }
}