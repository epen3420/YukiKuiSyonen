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
