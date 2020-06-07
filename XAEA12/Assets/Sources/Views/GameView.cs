using Sources.Photon;
using UnityEngine;

namespace Sources.Views
{
    public sealed class GameView : MonoBehaviour
    {
        private void Start()
        {
            IGameUserInteractions photonFacade = PhotonFacade.Instance;
            EventProxy.Instance.InvokeLater(new WaitForEndOfFrame(), NotifyGameSceneLoaded);

            void NotifyGameSceneLoaded()
            {
                photonFacade.SendGameSceneLoadedEvent();
            }
        }
    }
}
