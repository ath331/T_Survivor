using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomHolder : MonoBehaviour
{
    [SerializeField] private TMP_Text roomText;

    public void Refresh(string roomName = "")
    {
        roomText.text = $"방제 : {roomName} (인원 : ) [..]";
    }

    public void OnClickRoom()
    {
        // 1. 없는 방일 수 있음 => 자동 새로고침 해줌.
        // 2. 꽉차있는 방일 수 있음 => 자동 새로고침 해줌.
        // 3. 빈 방 => 입장


    }
}
