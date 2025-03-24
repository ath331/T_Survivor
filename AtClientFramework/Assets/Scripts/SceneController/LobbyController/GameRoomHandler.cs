using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameRoomHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;

    [SerializeField] private Transform[] spawnTransform;

    // ������ ĳ���� �ν��Ͻ��� �����ϴ� ����Ʈ
    private List<PlayerController> spawnedCharacters;

    // �� ���� ������ ��� ���θ� �����ϴ� �迭
    private bool[] spawnSlotsOccupied;

    ERoomState roomState = ERoomState.RoomStateNone;

    private int roomNumber = 0;

    private int cur_count = 1;

    private int max_count = 3;

    private string titleName = "";

    void OnDisable()
    {
        foreach (var character in spawnedCharacters)
        {
            ReturnCharacter(character);
        }
    }

    public void SetMaKeRoom(S_MakeRoom message)
    {
        var madeRoomInfo = message.MadeRoomInfo;

        titleName = madeRoomInfo.Name;

        roomNumber = madeRoomInfo.Num;

        roomState = madeRoomInfo.RoomState;

        cur_count = madeRoomInfo.CurCount;

        max_count = madeRoomInfo.MaxCount;

        titleText.text = $"[{roomNumber}] {titleName}";

        spawnedCharacters = new List<PlayerController>(max_count);

        spawnSlotsOccupied = new bool[max_count];

        Spawn_Character();
    }

    public void Spawn_Character()
    {
        for (int i = 0; i < max_count; i++)
        {
            // spawnSlotsOccupied �迭�� ����Ͽ� �ش� ������ ��� ������ Ȯ��
            if (!spawnSlotsOccupied[i])
            {
                PlayerController character = ObjectPoolManager.Instance.Get<PlayerController>("Knight", spawnTransform[i]);

                character.GetComponent<PlayerController>().enabled = false;
                character.rb.useGravity = false;

                // ������ ĳ���͸� ����Ʈ�� ����
                spawnedCharacters.Add(character);

                // �ش� ������ ��� ������ ǥ��
                spawnSlotsOccupied[i] = true;

                break;
            }
        }
    }

    public void Destroy_CharacterAtSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < spawnTransform.Length)
        {
            // �ش� ���Կ� �ڽ�(ĳ����)�� �ִ��� �Ǵ� spawnedCharacters ����Ʈ�� ������� Ȯ���� �� ����
            PlayerController character = spawnedCharacters.Find(ch => ch.transform.parent == spawnTransform[slotIndex]);

            if (character != null)
            {
                spawnedCharacters.Remove(character);

                ReturnCharacter(character);

                spawnSlotsOccupied[slotIndex] = false;
            }
        }
    }

    public void Destroy_Chracter()
    {
        foreach (var character in spawnedCharacters)
        {
            ReturnCharacter(character);
        }
    }

    public void ReturnCharacter(PlayerController character)
    {
        character.enabled = true;
        character.rb.useGravity = true;

        ObjectPoolManager.Instance.Return(character.gameObject);
    }
}
