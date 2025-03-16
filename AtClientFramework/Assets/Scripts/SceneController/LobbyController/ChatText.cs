using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatText : MonoBehaviour
{
    private void OnDestroy()
    {
        ObjectPoolManager.Instance.Return("ChatText", this.gameObject);
    }
}
