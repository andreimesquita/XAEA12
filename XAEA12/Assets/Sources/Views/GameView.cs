using System.Collections;
using Sources.Photon;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Views
{
    public sealed class GameView : MonoBehaviour
    {
        [SerializeField]
        private Button _playerButton;
        [SerializeField]
        private Animator _characterAnimator;
        
        private IEnumerator Start()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            PhotonFacade.Instance.OnStartGameSimulation += OnStartGameSimulation;
            PhotonFacade.Instance.OnButtonActiveStateChanged += OnButtonActiveStateChanged;
            PhotonFacade.Instance.OnTriggerAnimation += OnTriggerAnimation;
            OnPatternChanged(photonFacade.CurrentPattern);
            PhotonFacade.Instance.OnPatternChanged += OnPatternChanged;
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

        private void OnPatternChanged(int pattern)
        {
            //TODO(andrei) update slot states
        }

        private void OnStartGameSimulation()
        {
            //TODO(andrei) Start game simulation
            //TODO(andrei) Start character movement
            //TODO(andrei) players can press buttons
        }
    }
}
