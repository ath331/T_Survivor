using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void ReceiveMessage(string input)
    {
        text.text = input;
    }
    private void OnDestroy()
    {
        ObjectPoolManager.Instance.Return("ChatText", this.gameObject);
    }
}
