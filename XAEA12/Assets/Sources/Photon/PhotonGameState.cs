using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;

namespace Sources.Photon
{
    public class PhotonGameState
    {
        public Dictionary<int, Player> _playersById;
        public Dictionary<int, bool> _playersReadyById;
        public Dictionary<int, byte> _colorsByPlayerId;

        public List<byte> _availableColors;

        private int _currentSelection = 0000000000000000;
        private int _currentSelectionIndex;
        
        public PhotonGameState()
        {
            _playersById = new Dictionary<int, Player>();
            _colorsByPlayerId = new Dictionary<int, byte>();
            _playersReadyById = new Dictionary<int, bool>();
            
            _availableColors = new List<byte>();

            _availableColors.Add(GameEventHelper.Blue);
            _availableColors.Add(GameEventHelper.Red);
            _availableColors.Add(GameEventHelper.Green);
            _availableColors.Add(GameEventHelper.Yellow);
        }

        public void ResetTurn()
        {
            _currentSelection = 0000000000000000;
            _currentSelectionIndex = 0;
        }
        
        public void ButtonPress(int actorId)
        {
            byte actorColor = _colorsByPlayerId[actorId];
            _currentSelection = actorColor << (4 * _currentSelectionIndex);
            _currentSelectionIndex++;
        }

        public int CurrentSelection => _currentSelection;

        public bool ActionComplete()
        {
            return _currentSelectionIndex == 3;
        }

        public byte AddPlayer(Player player)
        {
            byte playerColor = GetUnusedColor();
            
            _playersById.Add(player.ActorNumber, player);
            _colorsByPlayerId.Add(player.ActorNumber, playerColor);
            return playerColor;
        }

        public byte GetUnusedColor()
        {
            byte color = _availableColors.First();
            _availableColors.Remove(color);
            return color;
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
    }
}