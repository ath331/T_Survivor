using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;

public class JobData
{
    public EPlayerType jobType { get; set; }
    public int HP { get; set; }
    public int MP { get; set; }
    public int Damage { get; set; }
    public int MagicDamage { get; set; }
}

public class ItemData
{
    public int Id { get; set; }
    public EPlayerType jobType { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public EEquipSlotType EquipSlotType { get; set; }
    public EStat Stat { get; set; }
    public int StatParam { get; set; }
}