using System;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip = null;

    private void Start()
    {
        SoundController.Instance.PlayMusic(_audioClip);
    }
}
