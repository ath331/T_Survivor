using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    public void PlayTestBGM()
    {
        SoundManager.Instance.PlayBGM("song1");
    }

    public void PlayTestSFX()
    {
        SoundManager.Instance.PlaySFX("breath");
    }
}
