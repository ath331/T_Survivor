using Cysharp.Threading.Tasks;
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

    public void PlayBGM(string name)
    {
        AudioClip clip = soundPreset?.bgmList.Find(bgm => bgm.name == name);

        if (clip != null)
        {
            if (bgmSource.isPlaying)
                bgmSource.Stop();

            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"[SoundManager] BGM '{name}' 을(를) 찾을 수 없습니다.");
        }
    }

    public async void PlaySFX(string name)
    {
        AudioClip clip = soundPreset?.sfxList.Find(sfx => sfx.name == name);

        if (clip != null)
        {
            await UniTask.Delay(50);
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] SFX '{name}' 을(를) 찾을 수 없습니다.");
        }
    }
}