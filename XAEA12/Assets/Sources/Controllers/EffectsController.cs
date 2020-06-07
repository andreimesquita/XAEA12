using UnityEngine;

public class EffectsController : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;

    public static EffectsController Instance
    {
        get => _instance;
        set => _instance = value;
    }

    private static EffectsController _instance = null;

    private void Awake()
    {
        _instance = this;
    }

    public void PlayExplosion(Vector3 position)
    {
        Instantiate(_explosion, position, Quaternion.identity);
    }
}
