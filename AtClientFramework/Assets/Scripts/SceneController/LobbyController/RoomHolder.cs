using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomHolder : MonoBehaviour
{
    [SerializeField] private TMP_Text roomText;

    public void Refresh(string roomName = "")
    {
        roomText.text = $"���� : {roomName} (�ο� : ) [..]";
    }

    public void OnClickRoom()
    {
        // 1. ���� ���� �� ���� => �ڵ� ���ΰ�ħ ����.
        // 2. �����ִ� ���� �� ���� => �ڵ� ���ΰ�ħ ����.
        // 3. �� �� => ����


    }
}
