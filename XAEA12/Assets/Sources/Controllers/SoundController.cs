using System;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _bgMusicSource = null;

    public static SoundController Instance
    {
        get => _instance;
        set => _instance = value;
    }

    private static SoundController _instance = null;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    public void PlayMusic(AudioClip clip)
    {
        _bgMusicSource.Stop();

        if (clip != null)
        {
            _bgMusicSource.clip = clip;
            _bgMusicSource.Play();
        }
    }
}
