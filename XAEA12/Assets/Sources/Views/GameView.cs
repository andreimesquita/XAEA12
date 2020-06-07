using System.Collections;
using Sources.Photon;
using UnityEngine;

namespace Sources.Views
{
    public sealed class GameView : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return null;
            IGameUserInteractions photonFacade = PhotonFacade.Instance;
            photonFacade.SendGameSceneLoadedEvent();
        }
    }
}
