using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    public CharacterData SelectedCharacter { get; private set; }

    public void SetCharacter(CharacterData character)
    {
        SelectedCharacter = character;
    }
}