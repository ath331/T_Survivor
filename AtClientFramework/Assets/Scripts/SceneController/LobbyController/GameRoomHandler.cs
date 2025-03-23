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

    ERoomState roomState = ERoomState.RoomStateNone;

    private int roomNumber = 0;

    private int cur_count = 1;

    private int max_count = 3;

    private string titleName = "";

    public void SetMaKeRoom(S_MakeRoom message)
    {
        var madeRoomInfo = message.MadeRoomInfo;

        titleName = madeRoomInfo.Name;

        roomNumber = madeRoomInfo.Num;

        roomState = madeRoomInfo.RoomState;

        cur_count = madeRoomInfo.CurCount;

        max_count = madeRoomInfo.MaxCount;

        titleText.text = $"[{roomNumber}] {titleName}";

        Spawn_Character();
    }

    public void Spawn_Character()
    {
        //GameObject character = ObjectPoolManager.Instance.Get("Knight");
        //character.SetActive(true);



    }

    public void Destroy_Chracter()
    {

    }

    public void OnClickSetting()
    {

    }

    public void OnClickEquipment()
    {

    }

    public void OnClickExit()
    {

    }
}
