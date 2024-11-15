using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem instance;

    [SerializeField]
    private List<SoundData> soundDataList;

    private Dictionary<string, SoundData> soundDataDict;
    private Queue<AudioSource> audioSourcePool;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeSoundDataDictionary();
        InitializeAudioSourcePool(10); // 初期プールサイズ
    }

    private void InitializeSoundDataDictionary()
    {
        soundDataDict = new Dictionary<string, SoundData>();
        foreach (var soundData in soundDataList)
            soundDataDict[soundData.title] = soundData;
    }

    private void InitializeAudioSourcePool(int initialSize)
    {
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < initialSize; i++)
            audioSourcePool.Enqueue(CreateAudioSource());
    }

    public void PlaySound(string title)
    {
        if (soundDataDict.TryGetValue(title, out var soundData))
        {
            var audioSource = GetOrCreateAudioSource();
            audioSource.clip = soundData.audioClip;
            audioSource.loop = soundData.soundType == SoundType.BGM;
            SetVolume(audioSource, soundData);

            if (audioSource.loop)
                audioSource.Play();
            else
                StartCoroutine(PlayAndRecycle(audioSource, soundData.audioClip.length));
        }
    }

    private AudioSource CreateAudioSource()
    {
        var audioSourceObject = new GameObject("PooledAudioSource");
        audioSourceObject.transform.SetParent(transform);
        return audioSourceObject.AddComponent<AudioSource>();
    }

    private AudioSource GetOrCreateAudioSource()
    {
        if (audioSourcePool.Count > 0)
        {
            var audioSource = audioSourcePool.Dequeue();
            audioSource.gameObject.SetActive(true);
            return audioSource;
        }
        return CreateAudioSource();
    }

    private IEnumerator PlayAndRecycle(AudioSource audioSource, float delay)
    {
        audioSource.PlayOneShot(audioSource.clip);
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.gameObject.SetActive(false);
        audioSourcePool.Enqueue(audioSource);
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
}
