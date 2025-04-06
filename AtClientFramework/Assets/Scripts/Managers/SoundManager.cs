using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private SoundPreset soundPreset;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    public static void Initialize()
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("SoundManager");
            Instance = obj.AddComponent<SoundManager>();
            DontDestroyOnLoad(obj);
            Instance.LoadPreset();
        }
    }

    private void LoadPreset()
    {
        soundPreset = Resources.Load<SoundPreset>("SoundPreset");
        if (soundPreset == null)
        {
            Debug.LogError("SoundPreset을 Resources 폴더에서 찾을 수 없습니다.");
        }

        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    //대충 요따가 PlayBGM()이나 PlaySFX() 만들기
}