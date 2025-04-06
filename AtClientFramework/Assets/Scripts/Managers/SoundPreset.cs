using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundPreset", menuName = "Audio/SoundPreset")]
public class SoundPreset : ScriptableObject
{
    public List<AudioClip> bgmList = new List<AudioClip>();
    public List<AudioClip> sfxList = new List<AudioClip>();
}
