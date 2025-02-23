using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JobData
{
    public string jobName;
    public float baseHealth;
    public float baseAttack;
    // 추가 데이터들
}

[Serializable]
public class ItemData
{
    public string itemName;
    public string itemType; // 예: "weapon", "armor" 등
    public float bonusAttack;
    // 추가 데이터들
}

// JsonUtility는 배열 디시리얼라이즈가 안 되므로 래퍼 클래스를 사용합니다.
[Serializable]
public class JobDataList
{
    public List<JobData> jobs;
}

[Serializable]
public class ItemDataList
{
    public List<ItemData> items;
}
