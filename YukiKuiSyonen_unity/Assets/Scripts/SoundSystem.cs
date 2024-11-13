using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Audio/SoundData")]
public class SoundData : ScriptableObject
{
    public string title;
    [Header("音量")]
    [Range(0, 1)] public float volume = 1.0f;
    public AudioClip audioClip;
    public SoundType soundType;
}

public enum SoundType
{
    BGM,
    SE,
    Voice
}

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem instance;


    [SerializeField]
    private List<SoundData> soundDataList;

    [SerializeField]
    private AudioSource bgmAudioSource;
    [SerializeField]
    private AudioSource seAudioSource;
    [SerializeField]
    private AudioSource voiceAudioSource;

    private Dictionary<string, SoundData> soundDataDict;
    private Dictionary<SoundType, AudioSource> audioSources;

    [Header("主音量")]
    [Range(0, 1)]
    [SerializeField]
    private float masterVolume = 1.0f;
    [Header("BGM音量")]
    [Range(0, 1)]
    [SerializeField]
    private float bgmVolume = 1.0f;
    [Header("SE音量")]
    [Range(0, 1)]
    [SerializeField]
    private float seVolume = 1.0f;
    [Header("ボイス音量")]
    [Range(0, 1)]
    [SerializeField]
    private float voiceVolume = 1.0f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeSoundDataDictionary();

        InitializeAudioSources();
    }

    private void InitializeSoundDataDictionary()
    {
        soundDataDict = new Dictionary<string, SoundData>();
        foreach (var soundData in soundDataList)
        {
            if (!soundDataDict.ContainsKey(soundData.title))
                soundDataDict.Add(soundData.title, soundData);
        }
    }

    private void InitializeAudioSources()
    {
        audioSources = new Dictionary<SoundType, AudioSource>
        {
            { SoundType.BGM, bgmAudioSource },
            { SoundType.SE, seAudioSource },
            { SoundType.Voice, voiceAudioSource }
        };
    }

    public void PlaySound(string title)
    {
        if (soundDataDict.TryGetValue(title, out var soundData))
        {
            var audioSource = audioSources[soundData.soundType];
            audioSource.clip = soundData.audioClip;
            SetVolume(audioSource, soundData);
            if (soundData.soundType == SoundType.BGM)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(soundData.audioClip);
            }
        }
    }

    public void AdjustMasterVolume(float newValue)
    {
        masterVolume = newValue;
        UpdateVolumes();
    }

    public void AdjustBgmVolume(float newValue)
    {
        bgmVolume = newValue;
        UpdateVolume(SoundType.BGM);
    }

    public void AdjustSeVolume(float newValue)
    {
        seVolume = newValue;
        UpdateVolume(SoundType.SE);
    }

    public void AdjustVoiceVolume(float newValue)
    {
        voiceVolume = newValue;
        UpdateVolume(SoundType.Voice);
    }

    private void SetVolume(AudioSource audioSource, SoundData soundData)
    {
        float categoryVolume = soundData.soundType switch
        {
            SoundType.BGM => bgmVolume,
            SoundType.SE => seVolume,
            SoundType.Voice => voiceVolume,
            _ => 1.0f
        };
        audioSource.volume = masterVolume * categoryVolume * soundData.volume;
    }

    private void UpdateVolumes()
    {
        foreach (var soundData in soundDataList)
        {
            var audioSource = audioSources[soundData.soundType];
            SetVolume(audioSource, soundData);
        }
    }

    private void UpdateVolume(SoundType type)
    {
        foreach (var soundData in soundDataList)
        {
            if (soundData.soundType == type)
            {
                var audioSource = audioSources[soundData.soundType];
                SetVolume(audioSource, soundData);
            }
        }
    }
}
