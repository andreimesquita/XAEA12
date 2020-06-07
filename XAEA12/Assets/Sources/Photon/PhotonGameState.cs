using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Sources.Photon
{
    public class PhotonGameState
    {
        public const string PLAYER_COLOR_FIELD = "player_color";
        
        private readonly HashSet<int> _playersReady = new HashSet<int>();
        private readonly HashSet<int> _playersWithLoadedScenes = new HashSet<int>();
        private  readonly List<byte> _availableColors = new List<byte>
        {
            GameEventHelper.Blue,
            GameEventHelper.Red,
            GameEventHelper.Green,
            GameEventHelper.Yellow
        };

        // MasterClient only data
        private int _currentSelection = 0000000000000000;
        private int _currentSelectionIndex;

        public void ResetTurn()
        {
            _currentSelection = 0000000000000000;
            _currentSelectionIndex = 0;
        }
        
        public void OnButtonPress(int actorId)
        {
            Room room = PhotonNetwork.CurrentRoom;
            Player player = room.GetPlayer(actorId);
            var hashtable = new Hashtable();
            if (!hashtable.TryGetValue(PLAYER_COLOR_FIELD, out object value)) return;
            byte color = (byte) value;
            _currentSelection = color << (4 * _currentSelectionIndex);
            _currentSelectionIndex++;
        }
        
        public bool TryGetPlayerColor(int actorId, out byte color)
        {
            Room room = PhotonNetwork.CurrentRoom;
            Player player = room.GetPlayer(actorId);
            Hashtable hashtable = player.CustomProperties;
            if (hashtable.TryGetValue(PLAYER_COLOR_FIELD, out object value))
            {
                color = (byte) value;
                return true;
            }
            color = 0;
            return false;
        }

        public int CurrentSelection => _currentSelection;

        public bool ActionComplete()
        {
            return _currentSelectionIndex == 3;
        }

        public byte GetUnusedColor()
        {
            byte color = _availableColors.First();
            _availableColors.Remove(color);
            return color;
        }
        
        public bool RoomIsFilled()
        {
            Room room = PhotonNetwork.CurrentRoom;
            return room.PlayerCount == 4;
        }

        public void SetPlayerReady(int actorNumber)
        {
            _playersReady.Add(actorNumber);
        }

        public bool IsPlayerReady(int actorNumber)
        {
            return _playersReady.Contains(actorNumber);
        }

        public bool AllPlayersReady()
        {
            return _playersReady.Count == 4;
        }

        public void SetPlayerGameSceneLoaded(int actorNumber)
        {
            _playersWithLoadedScenes.Add(actorNumber);
        }

        public bool AreAllGameScenesLoaded()
        {
            return _playersWithLoadedScenes.Count == 4;
        }
    }
}