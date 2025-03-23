using Assets.Scripts.Network;
using Protocol;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
    public static Action<string> OnChatReceived;

    [SerializeField] private GameObject content;
    [SerializeField] private TMP_InputField inputField;

    private const int LimitChatPrefab = 20;
    private const int LimitChatLength = 50;

    // 해야 될 것 inputfield에 입력해서 엔터/입력 버튼을 누를 때 채팅을 전송하고 content안에 자식 프리팹을 생성해서 넣어야 한다!

    private void Start()
    {
        inputField.characterLimit = LimitChatLength;
        inputField.onEndEdit.AddListener(OnEndEdit);

        OnChatReceived += UpdateChatUI;
    }

    private void OnEndEdit(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Send_Message(input);
        }
    }

    private void Submit(string input)
    {
        if (inputField.isFocused && Input.GetKeyDown(KeyCode.Return))   // 채팅창을 누르고 엔터키를 누를 시
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

    private void OnDestroy()
    {
        OnChatReceived -= UpdateChatUI;
    }

    public void UpdateChatUI(string message)
    {
        if (content.transform.childCount >= LimitChatPrefab)
        {
            GameObject oldestChat = content.transform.GetChild(0).gameObject;
            oldestChat.SetActive(false);  // 먼저 비활성화
            ObjectPoolManager.Instance.Return("ChatText", oldestChat); // 비활성화 후 오브젝트 풀로 반환
            //Destroy(oldestChat) 이거 했다가 오류났음
        }

        GameObject chatText = ObjectPoolManager.Instance.Get("ChatText");
        chatText.transform.parent = content.transform;
        chatText.SetActive(true);

        chatText.GetComponent<ChatText>().ReceiveMessage(message);
    }
}