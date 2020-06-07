using System.Collections;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private SfxPlayer _sfxPlayer = null;
    
    void Awake()
    {
        _sfxPlayer.Play();
        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

}
