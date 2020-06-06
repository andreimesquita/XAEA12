using System;
using UnityEngine;

namespace Sources.Photon
{
    public class PhotonTest : MonoBehaviour
    {
        [SerializeField]
        private string _userName;
        
        private void Start()
        {
            PhotonServer launcher = new PhotonServer();
            launcher.JoinGame(_userName);
        }
    }
}