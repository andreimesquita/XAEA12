using UnityEngine;

namespace Sources.Photon
{
    public class PhotonTest : MonoBehaviour
    {
        [SerializeField]
        private string _userName;
        
        private void Start()
        {
            PhotonServer.Instance.JoinGame(_userName);
        }
    }
}
