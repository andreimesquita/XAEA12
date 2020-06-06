using System.Collections.Generic;
using Photon.Realtime;

namespace Sources.Photon
{
    public class PhotonGameState
    {
        public readonly List<Player> Players;

        public PhotonGameState()
        {
            Players = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
    }
}