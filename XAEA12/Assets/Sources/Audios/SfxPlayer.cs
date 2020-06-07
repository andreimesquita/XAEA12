using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips = null;
    [SerializeField] private AudioSource _source = null;
    [SerializeField] private bool _randomizePitch = false;
    [SerializeField] private float _pitchRange = 0.1f;
    
    private void Awake()
    {
        if (_source == null)
            _source = GetComponent<AudioSource>();
    }

    public virtual void Play()
    {
        int index = Random.Range(0, _clips.Length);
        AudioClip clip = _clips[index];
        PlayClip(clip);
    }

    protected void PlayClip(AudioClip clip)
    {
        float pitch = 1.0f;
        
        if (_randomizePitch)
            pitch = Random.Range(1.0f - _pitchRange, 1.0f + _pitchRange);

        _source.pitch = pitch;
        _source.PlayOneShot(clip);
    }
}
