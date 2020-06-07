using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;

namespace Sources.Photon
{
    public class PhotonGameState
    {
        public Dictionary<int, Player> _playersById;
        public Dictionary<byte, Player> _playerByColorPattern;
        public Dictionary<int, bool> _playersReadyById;
        public Dictionary<int, bool> _loadedGameScenesReadyByPlayerId;
        public Dictionary<int, byte> _colorsByPlayerId;

        public List<byte> _availableColors;

        private int _currentSelection = 0000000000000000;
        private int _currentSelectionIndex;

        public PhotonGameState()
        {
            _playersById = new Dictionary<int, Player>();
            _colorsByPlayerId = new Dictionary<int, byte>();
            _playersReadyById = new Dictionary<int, bool>();
            _playerByColorPattern = new Dictionary<byte, Player>();

            _availableColors = new List<byte>
            {
                GameEventHelper.Blue,
                GameEventHelper.Red,
                GameEventHelper.Green,
                GameEventHelper.Yellow
            };
        }

        public void ResetTurn()
        {
            _currentSelection = 0000000000000000;
            _currentSelectionIndex = 0;
        }
        
        public void ButtonPress(int actorId)
        {
            byte actorColor = _colorsByPlayerId[actorId];
            _playerByColorPattern[actorColor] = _playersById[actorId];
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
            _playersById[player.ActorNumber] = player;
            _colorsByPlayerId[player.ActorNumber] = playerColor;
            return playerColor;
        }

        private byte GetUnusedColor()
        {
            byte color = _availableColors.First();
            _availableColors.Remove(color);
            return color;
        }
        
        public bool RoomIsFilled()
        {
            return _playersById.Count == 4;
        }

        public void SetPlayerReady(int actorNumber, bool isReady)
        {
            _playersReadyById[actorNumber] = isReady;
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

        public void SetGameSceneReady(int actorNumber)
        {
            _loadedGameScenesReadyByPlayerId[actorNumber] = true;
        }

        public bool AreAllGameScenesLoaded()
        {
            foreach (KeyValuePair<int,bool> loadedScenes in _loadedGameScenesReadyByPlayerId)
            {
                if (!loadedScenes.Value) return false;
            }
            return true;
        }
    }
}