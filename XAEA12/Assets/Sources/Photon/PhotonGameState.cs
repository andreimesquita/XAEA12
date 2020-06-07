using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;

namespace Sources.Photon
{
    public class PhotonGameState
    {
        public readonly Dictionary<int, Player> _playersById = new Dictionary<int, Player>();
        public readonly Dictionary<byte, Player> _playerByColorPattern = new Dictionary<byte, Player>();
        public readonly Dictionary<int, bool> _playersReadyById = new Dictionary<int, bool>();
        public readonly Dictionary<int, byte> _colorsByPlayerId = new Dictionary<int, byte>();

        private readonly Dictionary<int, bool> _loadedGameScenesReadyByPlayerId = new Dictionary<int, bool>();
        private  readonly List<byte> _availableColors = new List<byte>
        {
            GameEventHelper.Blue,
            GameEventHelper.Red,
            GameEventHelper.Green,
            GameEventHelper.Yellow
        };

        private int _currentSelection = 0000000000000000;
        private int _currentSelectionIndex;

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