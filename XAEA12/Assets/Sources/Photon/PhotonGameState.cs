using System.Collections.Generic;
using Photon.Realtime;

namespace Sources.Photon
{
    public class PhotonGameState
    {
        public Dictionary<int, Player> _playersById;
        public Dictionary<int, bool> _playersReadyById;
        public Dictionary<int, byte> _colorsByPlayerId;
        
        public PhotonGameState()
        {
            _playersById = new Dictionary<int, Player>();
            _colorsByPlayerId = new Dictionary<int, byte>();
        }

        public void AddPlayer(Player player)
        {
            _playersById.Add(player.ActorNumber, player);
        }
        
        public bool RoomIsFilled()
        {
            return _playersById.Count == 4;
        }

        public void SetPlayerReady(int actorNumber)
        {
            _playersReadyById.Add(actorNumber, true);
        }

        public bool AllPlayersReady()
        {
            foreach (var ready in _playersReadyById)
            {
                if (!ready.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public void SetPlayerColor(int actorNumber, byte color)
        { 
            _colorsByPlayerId.Add(actorNumber, color);
        }
    }
}