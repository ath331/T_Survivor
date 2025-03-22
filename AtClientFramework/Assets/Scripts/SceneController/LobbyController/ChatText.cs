using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void ReceiveMessage(string input)
    {
        input = InsertLineBreak(input, 30);
        text.text = input;
    }

    private string InsertLineBreak(string input, int lineLimit)
    {
        if (input.Length <= lineLimit)
        {
            return input;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int count = 0;

        foreach (char c in input)
        {
            sb.Append(c);
            count++;

            // 30자마다 줄 바꿈 추가
            if (count >= lineLimit)
            {
                sb.Append("\n");
                count = 0;
            }
        }

        return sb.ToString();
    }
}