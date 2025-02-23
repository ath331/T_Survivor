using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJob
{
    string JobName { get; }

    void Initialize(PlayerController player);
    
    void UseSkill(Skill skill);
}
