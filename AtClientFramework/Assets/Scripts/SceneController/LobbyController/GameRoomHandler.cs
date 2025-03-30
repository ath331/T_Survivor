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

    // 생성된 캐릭터 인스턴스를 보관하는 리스트
    private List<PlayerController> spawnedCharacters;

    // 각 스폰 슬롯의 사용 여부를 관리하는 배열
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
            // spawnSlotsOccupied 배열을 사용하여 해당 슬롯이 사용 중인지 확인
            if (!spawnSlotsOccupied[i])
            {
                PlayerController character = ObjectPoolManager.Instance.Get<PlayerController>("Knight", spawnTransform[i]);

                character.GetComponent<PlayerController>().enabled = false;
                character.rb.useGravity = false;

                // 생성된 캐릭터를 리스트에 보관
                spawnedCharacters.Add(character);

                // 해당 슬롯을 사용 중으로 표시
                spawnSlotsOccupied[i] = true;

                break;
            }
        }
    }

    public void Destroy_CharacterAtSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < spawnTransform.Length)
        {
            // 해당 슬롯에 자식(캐릭터)이 있는지 또는 spawnedCharacters 리스트를 기반으로 확인할 수 있음
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
