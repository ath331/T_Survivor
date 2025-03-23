using Assets.Scripts.Network;
using Protocol;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{ 
    [SerializeField] private GameObject content;
    [SerializeField] private TMP_InputField inputField;

    private const int LimitChatPrefab = 20;
    private const int LimitChatLength = 50;

    private List<ChatText> chatTexts = new List<ChatText>();

    private void OnEnable()
    {
        Chat_Strategy.OnChatReceived += UpdateChatUI;
    }

    private void Start()
    {
        inputField.characterLimit = LimitChatLength;
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnDisable()
    {
        Chat_Strategy.OnChatReceived -= UpdateChatUI;
    }

    private void OnDestroy()
    {
        foreach(var chatText in chatTexts)
        {
            if(chatText != null)
            {
                ObjectPoolManager.Instance.Return("ChatText", chatText.gameObject);
            }
        }
        chatTexts.Clear();
    }

    private void OnEndEdit(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Send_Message(input);
        }
    }

    public void Send_Message(string input)
    {
        string msg = input.Trim();

        // 비어있는 메시지는 보내지 않음.
        if (string.IsNullOrEmpty(msg)) return;

        ulong id = MercuryHelper.mercuryId;

        string defaultMessage = $"{id} : {msg}";

        C_Chat pkt = new C_Chat();

        pkt.Msg = defaultMessage;

        inputField.text = string.Empty;
        inputField.ActivateInputField();

        NetworkManager.Instance.Send(pkt);
    }

    public void UpdateChatUI(string message)
    {
        if (content.transform.childCount >= LimitChatPrefab)
        {
            ChatText oldestChat = chatTexts[0];
            oldestChat.gameObject.SetActive(false);  // 먼저 비활성화
            ObjectPoolManager.Instance.Return(oldestChat.gameObject); // 비활성화 후 오브젝트 풀로 반환
            chatTexts.RemoveAt(0);
        }

        GameObject chatTextObj = ObjectPoolManager.Instance.Get("ChatText");
        chatTextObj.transform.parent = content.transform;
        chatTextObj.SetActive(true);

        ChatText chatText = chatTextObj.GetComponent<ChatText>();
        chatText.ReceiveMessage(message);

        chatTexts.Add(chatText);
    }
}