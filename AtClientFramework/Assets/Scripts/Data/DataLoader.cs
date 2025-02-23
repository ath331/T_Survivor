using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    // �����Ϳ��� �Ҵ� (Resources ���� ��뵵 ����) �ϴ� �밭 ��Ƴ�
    public TextAsset jobJson;
    public TextAsset itemJson;

    public List<JobData> jobDataList;
    public List<ItemData> itemDataList;

    void LoadData()
    {
        JobDataList jobWrapper = JsonUtility.FromJson<JobDataList>(jobJson.text);
        jobDataList = jobWrapper.jobs;

        ItemDataList itemWrapper = JsonUtility.FromJson<ItemDataList>(itemJson.text);
        itemDataList = itemWrapper.items;

        Debug.Log("JobData�� ItemData �ε� �Ϸ�!");
    }
}
