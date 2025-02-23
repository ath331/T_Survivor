using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericJob : IJob
{
    private JobData jobData;
    public string JobName => jobData.jobName;

    public GenericJob(JobData data)
    {
        jobData = data;
    }

    public void Initialize(PlayerController player)
    {
        Debug.Log($"Initializing job: {JobName} with base health {jobData.baseHealth} and base attack {jobData.baseAttack}");
        // 예시: player.Health = jobData.baseHealth;
        // 외부 데이터에 따른 추가 초기화 로직 구현 가능
    }

    public void UseSkill(Skill skill)
    {
        //Debug.Log($"{JobName} uses skill: {skill.Name}");
        //skill.Use();
    }
}
