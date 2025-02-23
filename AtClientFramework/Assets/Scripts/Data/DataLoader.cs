using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    // 에디터에서 할당 (Resources 폴더 사용도 가능) 일단 대강 잡아놈
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

        Debug.Log("JobData와 ItemData 로딩 완료!");
    }
}
