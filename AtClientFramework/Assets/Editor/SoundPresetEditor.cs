using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(SoundPreset))]
public class SoundPresetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SoundPreset preset = (SoundPreset)target;

        if (GUILayout.Button("Import"))
        {
            ImportSounds(preset);
        }
    }

    private void ImportSounds(SoundPreset preset)
    {
        preset.bgmList.Clear();
        preset.sfxList.Clear();

        string bgmPath = "Assets/Resources/Sounds/BGM";
        string sfxPath = "Assets/Resources/Sounds/SFX";

        LoadAudioClips(bgmPath, preset.bgmList);
        LoadAudioClips(sfxPath, preset.sfxList);

        EditorUtility.SetDirty(preset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void LoadAudioClips(string path, List<AudioClip> list)
    {
        if (!Directory.Exists(path)) return;

        string[] files = Directory.GetFiles(path, "*.mp3");

        foreach (string file in files)
        {
            string assetPath = file.Replace("\\", "/"); // 경로 구분자 통일

            // "Assets/Resources/" 부분 제거
            if (!assetPath.StartsWith("Assets/Resources/"))
            {
                Debug.LogWarning($"Invalid path: {assetPath}");
                continue;
            }

            // Resources.Load()에서 사용할 경로로 변환
            string resourcesPath = assetPath.Replace("Assets/Resources/", "").Replace(".mp3", "");
            Debug.Log($"Processed resources path: {resourcesPath}");

            // AudioClip 로드
            AudioClip clip = Resources.Load<AudioClip>(resourcesPath);
            if (clip != null)
            {
                list.Add(clip);
                Debug.Log($"Loaded: {resourcesPath}");
            }
            else
            {
                Debug.LogWarning($"Failed to load AudioClip: {resourcesPath}");
            }
        }
    }
}
