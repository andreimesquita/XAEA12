using UnityEngine;

public class RandomGroupsSfxPlayer : SfxPlayer
{
    [SerializeField] private AudioClip[] _priorityClips = null;
    [SerializeField] private float _priorotyChance = 0.9f;
    
    
    public override void Play()
    {
        bool playPriorityClips = Random.Range(0.0f, 1.0f) < _priorotyChance;

        if (playPriorityClips)
        {
            int index = Random.Range(0, _priorityClips.Length);
            AudioClip clip = _priorityClips[index];
            base.PlayClip(clip);
        }
        else
        {
            base.Play();
        }
    }
}
