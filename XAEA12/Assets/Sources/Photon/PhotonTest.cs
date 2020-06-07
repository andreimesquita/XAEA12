using UnityEngine;

namespace Sources.Photon
{
    public class PhotonTest : MonoBehaviour
    {
        [SerializeField]
        private string _userName;
        
        private void Start()
        {
            PhotonFacade.Instance.Login(_userName);
        }
    }
}
