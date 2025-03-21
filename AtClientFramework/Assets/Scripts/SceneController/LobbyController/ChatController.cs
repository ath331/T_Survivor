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

    // �ؾ� �� �� inputfield�� �Է��ؼ� ����/�Է� ��ư�� ���� �� ä���� �����ϰ� content�ȿ� �ڽ� �������� �����ؼ� �־�� �Ѵ�!

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
        if (inputField.isFocused && Input.GetKeyDown(KeyCode.Return))   // ä��â�� ������ ����Ű�� ���� ��
        {
            Send_Message(input);
        }
    }

    public void Send_Message(string input)
    {
        string msg = input.Trim();

        // ����ִ� �޽����� ������ ����.
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
            oldestChat.SetActive(false);  // ���� ��Ȱ��ȭ
            ObjectPoolManager.Instance.Return("ChatText", oldestChat); // ��Ȱ��ȭ �� ������Ʈ Ǯ�� ��ȯ
            //Destroy(oldestChat) �̰� �ߴٰ� ��������
        }

        GameObject chatText = ObjectPoolManager.Instance.Get("ChatText");
        chatText.transform.parent = content.transform;
        chatText.SetActive(true);

        chatText.GetComponent<ChatText>().ReceiveMessage(message);
    }
}