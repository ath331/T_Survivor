using System.Collections;
using System.Collections.Generic;
using Protocol;
using TMPro;
using UnityEngine;

public class RoomHolder : MonoBehaviour
{
    [SerializeField] private TMP_Text roomText;

    ERoomState roomState = ERoomState.RoomStateNone;
    
    private string titleName = "";

    private int roomNumber = 0;

    private int cur_count = 1;

    private int max_count = 3;


    public void SetStatus(S_MakeRoom message)
    {
        var madeRoomInfo = message.MadeRoomInfo;

        roomState = madeRoomInfo.RoomState;

        titleName = madeRoomInfo.Name;

        roomNumber = madeRoomInfo.Num;

        cur_count = madeRoomInfo.CurCount;

        max_count = madeRoomInfo.MaxCount;

        roomText.text = $"방제 : {titleName} (인원: {cur_count}/{max_count}) [{roomState.ToString()}]";
    }

    // 1. 없는 방일 수 있음
    // 2. 꽉차있는 방일 수 있음
    // 3. 빈 방 => 입장
    public void OnClickRoom()
    {
        


    }
}
