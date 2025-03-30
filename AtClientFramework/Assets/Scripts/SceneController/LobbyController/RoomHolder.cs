using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class RoomHolder : MonoBehaviour
{
    [SerializeField] private TMP_Text roomText;

    string roomState = "대기중";
    
    private string titleName = "";

    private int roomNumber = 0;

    private int cur_count = 1;

    private int max_count = 3;


    public void SetStatus(S_RequestRoomInfo message)
    {
        var madeRoomInfo = message.RoomInfo;

        titleName = madeRoomInfo.Name;

        roomNumber = madeRoomInfo.Num;

        cur_count = madeRoomInfo.CurCount;

        max_count = madeRoomInfo.MaxCount;

        roomState = madeRoomInfo.RoomState == ERoomState.RoomStateWaiting ? "대기중" : "게임중";

        roomText.text = $"{titleName} (인원: {cur_count}/{max_count}) <#000000>[{roomState}]";
    }

    // 1. 없는 방일 수 있음
    // 2. 꽉차있는 방일 수 있음
    // 3. 비번방 일수도 (일단 아니라고 가정)
    // 4. 빈 방 => 입장
    public void OnClickRoom()
    {
        // 룸 넘버만 보내주면 되려나? 받아서 핸들링하면 되니까
        C_WaitingRoomEnter enterRoom = new C_WaitingRoomEnter()
        {
            RoomInfo = new RoomInfo
            {
                Num = roomNumber,

                Name = titleName,
                
                Pw = -1,
            }
        };

        NetworkManager.Instance.Send(enterRoom);
    }
}
