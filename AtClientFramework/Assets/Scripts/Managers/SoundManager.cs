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
            Debug.LogError("SoundPreset�� Resources �������� ã�� �� �����ϴ�.");
        }

        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    //���� ����� PlayBGM()�̳� PlaySFX() �����
}