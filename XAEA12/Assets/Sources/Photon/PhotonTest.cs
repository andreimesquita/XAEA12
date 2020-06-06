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
            PhotonLauncher launcher = new PhotonLauncher(_userName);
            launcher.Connect();
        }
    }
}