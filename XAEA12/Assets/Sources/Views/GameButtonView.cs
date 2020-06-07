using Sources.Photon;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Views
{
    public sealed class GameButtonView : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        
        private void Start()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            photonFacade.OnButtonActiveStateChanged += OnButtonActiveStateChanged;
            photonFacade.OnStartGameSimulation += OnStartGameSimulation;
        }

        private void OnStartGameSimulation()
        {
            OnButtonActiveStateChanged(true);
        }
        
        private void OnButtonActiveStateChanged(bool isActive)
        {
            _button.interactable = isActive;
        }
    }
}
