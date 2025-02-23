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
        // ����: player.Health = jobData.baseHealth;
        // �ܺ� �����Ϳ� ���� �߰� �ʱ�ȭ ���� ���� ����
    }

    public void UseSkill(Skill skill)
    {
        //Debug.Log($"{JobName} uses skill: {skill.Name}");
        //skill.Use();
    }
}
