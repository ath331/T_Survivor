using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private SoundPreset soundPreset;

    private AudioSource bgmSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();
    private int sfxSourceCount = 5; // 동시 재생 가능한 효과음 수

    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    private Dictionary<string, float> sfxCoolDowns = new Dictionary<string, float>  // 넣을 효과음 이름과 쿨타임
    {
        { "breath", 0.15f },
        { "canFall", 0f }
    };

    private Dictionary<string, float> sfxLastPlayed = new Dictionary<string, float>();

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
            return;
        }

        foreach(var clip in soundPreset.bgmList)
        {
            if (!bgmDict.ContainsKey(clip.name))
            {
                bgmDict.Add(clip.name, clip);
            }
        }

        foreach(var clip in soundPreset.sfxList)
        {
            if (!sfxDict.ContainsKey(clip.name))
            {
                sfxDict.Add(clip.name, clip);
            }
        }

        bgmSource = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < sfxSourceCount; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(src);
        }
    }

    public void PlayBGM(string name)
    {
        if (bgmDict.TryGetValue(name, out AudioClip clip))
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }

            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"[SoundManager] BGM '{name}' 을(를) 찾을 수 없습니다.");
        }
    }

    public void PlaySFX(string name)
    {
        if(!sfxDict.TryGetValue(name, out AudioClip clip))
        {
            Debug.LogWarning($"[SoundManager] SFX '{name}' 을(를) 찾을 수 없습니다.");
            return;
        }

        float now = Time.time;
        if (sfxCoolDowns.TryGetValue(name, out float cooldown))
        {
            if (sfxLastPlayed.TryGetValue(name, out float lastTime) && (now - lastTime) < cooldown)
            {
                return;
            }
        }

        sfxLastPlayed[name] = now;

        AudioSource source = GetAvailableSFXSource();
        if (source != null)
        {
            source.PlayOneShot(clip);
        }
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (var src in sfxSources)
        {
            if (!src.isPlaying)
            {
                return src;
            }
        }

        return sfxSources[0];
    }
}