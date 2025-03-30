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
    // 3. ����� �ϼ��� (�ϴ� �ƴ϶�� ����)
    // 4. �� �� => ����
    public void OnClickRoom()
    {
        // �� �ѹ��� �����ָ� �Ƿ���? �޾Ƽ� �ڵ鸵�ϸ� �Ǵϱ�
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
