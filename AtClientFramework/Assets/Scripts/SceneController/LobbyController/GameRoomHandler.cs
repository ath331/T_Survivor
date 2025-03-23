using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;
using UnityEngine;

public class GameRoomHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;

    [SerializeField] private Transform[] spawnTransform;

    private List<GameObject> spawnedCharacters;

    ERoomState roomState = ERoomState.RoomStateNone;

    private int roomNumber = 0;

    private int cur_count = 1;

    private int max_count = 3;

    private string titleName = "";

    void OnDisable()
    {
        foreach (var character in spawnedCharacters)
        {
            ObjectPoolManager.Instance.Return(character);
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

        spawnedCharacters = new List<GameObject>(max_count);

        Spawn_Character();
    }

    public void Spawn_Character()
    {
        GameObject character = ObjectPoolManager.Instance.Get("Knight", spawnTransform[0]);
        spawnedCharacters.Add(character);

        for (int i = 0; i < max_count; i++)
        {

        }
    }

    public void Destroy_Chracter()
    {
        foreach (var character in spawnedCharacters)
        {
            ObjectPoolManager.Instance.Return(character);
        }
    }
}
