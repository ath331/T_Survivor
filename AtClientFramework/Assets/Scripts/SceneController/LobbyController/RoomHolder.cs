using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;
using UnityEngine;

public class RoomHolder : MonoBehaviour
{
    [SerializeField] private TMP_Text roomText;

    private string roomState = "�����";
    
    private string titleName = "";

    public int RoomNumber { get; set; }

    private int cur_count = 1;

    private int max_count = 3;

    public void SetStatus(RoomInfo roomInfo)
    {
        titleName = roomInfo.Name;

        RoomNumber = roomInfo.Num;

        cur_count = roomInfo.CurCount;

        max_count = roomInfo.MaxCount;

        roomState = roomInfo.RoomState == ERoomState.RoomStateWaiting ? "�����" : "������";

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
                Num = RoomNumber,

                Name = titleName,
                
                Pw = -1,
            }
        };

        NetworkManager.Instance.Send(enterRoom);
    }
}
