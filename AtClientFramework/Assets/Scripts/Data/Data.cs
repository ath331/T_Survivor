using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JobData
{
    public string jobName;
    public float baseHealth;
    public float baseAttack;
    // �߰� �����͵�
}

[Serializable]
public class ItemData
{
    public string itemName;
    public string itemType; // ��: "weapon", "armor" ��
    public float bonusAttack;
    // �߰� �����͵�
}

// JsonUtility�� �迭 ��ø������� �� �ǹǷ� ���� Ŭ������ ����մϴ�.
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
