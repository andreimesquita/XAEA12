using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _bgMusicSource = null;
    [SerializeField] private AudioSource _sfxSource = null;

    public static SoundController Instance
    {
        get => _instance;
        set => _instance = value;
    }

    private static SoundController _instance = null;

    public void PlayMusic(AudioClip clip)
    {
        _bgMusicSource.Stop();
        _bgMusicSource.clip = clip;
        _bgMusicSource.Play();
    }
}
