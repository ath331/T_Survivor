using Assets.Scripts.Network;
using Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatController : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private TMP_InputField inputField;

    private const int LimitChatPrefab = 20;
    private const int LimitChatLength = 50;

    // 해야 될 것 inputfield에 입력해서 엔터/입력 버튼을 누를 때 채팅을 전송하고 content안에 자식 프리팹을 생성해서 넣어야 한다!

    private void Update()
    {
        if (inputField.isFocused && Input.GetKeyDown(KeyCode.Return))   // 채팅창을 누르고 엔터키를 누를 시
        {
            Send_Message();
        }
    }

    public void Send_Message()
    {
        string msg = inputField.text.Trim();

        // 비어있는 메시지는 보내지 않음.
        if (string.IsNullOrEmpty(msg)) return;

        GameObject chatText = ObjectPoolManager.Instance.Get("ChatText");
        chatText.transform.parent = content.transform;
        chatText.SetActive(true);

        C_Chat pkt = new C_Chat();

        pkt.Msg = msg;

        inputField.text = string.Empty;
        inputField.ActivateInputField();

        NetworkManager.Instance.Send(pkt);
    }
}
