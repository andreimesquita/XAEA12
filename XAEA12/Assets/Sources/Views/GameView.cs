using System.Collections;
using Sources.Photon;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sources.Views
{
    public sealed class GameView : MonoBehaviour
    {
        [SerializeField]
        private Button _playerButton;
        [SerializeField]
        private Animator _characterAnimator;
        [SerializeField]
        public UnityEvent _onSimulationStarted;
        
        private IEnumerator Start()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            PhotonFacade.Instance.OnStartGameSimulation += OnStartGameSimulation;
            PhotonFacade.Instance.OnButtonActiveStateChanged += OnButtonActiveStateChanged;
            PhotonFacade.Instance.OnTriggerAnimation += OnTriggerAnimation;
            yield return null;
            photonFacade.SendGameSceneLoadedEvent();
        }

        private void OnTriggerAnimation(string trigger)
        {
            int hash = Animator.StringToHash(trigger);
            _characterAnimator.SetTrigger(hash);
        }

        private void OnButtonActiveStateChanged(bool isActive)
        {
            _playerButton.interactable = isActive;
        }

        private void OnStartGameSimulation()
        {
            _onSimulationStarted.Invoke();
        }
    }
}
