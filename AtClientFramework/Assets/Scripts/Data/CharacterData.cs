using UnityEngine;
using Protocol;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Data/Character Data")]
public class CharacterData : ScriptableObject
{
    public EPlayerType jobType;

    public string prefabName;

    public GameObject characterPrefab;
}