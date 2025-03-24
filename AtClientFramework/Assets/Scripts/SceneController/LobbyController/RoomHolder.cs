using System.Collections;
using System.Collections.Generic;
using Protocol;
using TMPro;
using UnityEngine;

public class RoomHolder : MonoBehaviour
{
    [SerializeField] private TMP_Text roomText;

    string roomState = "�����";
    
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

        roomState = madeRoomInfo.RoomState == ERoomState.RoomStateWaiting ? "�����" : "������";

        roomText.text = $"{titleName} (�ο�: {cur_count}/{max_count}) <#000000>[{roomState}]";
    }

    // 1. ���� ���� �� ����
    // 2. �����ִ� ���� �� ����
    // 3. �� �� => ����
    public void OnClickRoom()
    {
        


    }
}
