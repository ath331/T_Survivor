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

        roomText.text = $"���� : {titleName} (�ο�: {cur_count}/{max_count}) [{roomState.ToString()}]";
    }

    // 1. ���� ���� �� ����
    // 2. �����ִ� ���� �� ����
    // 3. �� �� => ����
    public void OnClickRoom()
    {
        


    }
}
