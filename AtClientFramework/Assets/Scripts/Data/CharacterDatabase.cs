using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Protocol;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Data/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    public List<CharacterData> characters;

    public CharacterData GetCharacter(EPlayerType jobType)
    {
        return characters.FirstOrDefault(c => c.jobType == jobType);
    }
}