using Sources.Photon;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        
        PhotonFacade photonFacade = PhotonFacade.Instance;
        photonFacade.OnTriggerAnimation += SetCommand;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetCommand("Left");
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetCommand("Right");
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SetCommand("Jump");
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SetCommand("Air");
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SetCommand("Bazooka");
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            SetCommand("Laser");
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            SetCommand("Fire");
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            SetCommand("Explode");
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SetCommand("Water");
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            SetCommand("Death");
        else if (Input.GetKeyDown(KeyCode.A))
            SetCommand("Endgame");
    }

    public void SetCommand(string trigger)
    {
        if (SimulationController.Instance.Initialized)
            _animator.SetTrigger(trigger);
    }
}
