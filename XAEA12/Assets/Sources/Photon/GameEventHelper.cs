using System.Collections.Generic;

namespace Sources.Photon
{
    public class GameEventHelper
    {
        private Dictionary<byte, EVENT_CODES> _eventCodeDic;
        private Dictionary<EVENT_CODES, byte> _eventTypeDic;

        // Button Colors
        public const byte Blue = 1 << 0;
        public const byte Green = 1 << 1;
        public const byte Red = 1 << 2;
        public const byte Yellow = 1 << 3;

        public GameEventHelper()
        {
            _eventCodeDic = new Dictionary<byte, EVENT_CODES>();
            _eventTypeDic = new Dictionary<EVENT_CODES, byte>();
            
            addEventToMap(EVENT_CODES.INVALID, 1);
            addEventToMap(EVENT_CODES.SET_COLOR, 2);
            addEventToMap(EVENT_CODES.PLAYER_READY, 3);
            addEventToMap(EVENT_CODES.PRESS_BUTTON, 4);
            addEventToMap(EVENT_CODES.PLAYER_JOINED, 5);
            addEventToMap(EVENT_CODES.ROOM_IS_FILLED, 6);
            addEventToMap(EVENT_CODES.ALL_PLAYERS_READY, 7);
            addEventToMap(EVENT_CODES.GAME_STARTED, 8);
            addEventToMap(EVENT_CODES.TURN_STARTED, 9);
            addEventToMap(EVENT_CODES.TURN_FINISHED, 10);
            addEventToMap(EVENT_CODES.TURN_RESET, 11);
        }

        public void addEventToMap(EVENT_CODES eventType, byte code)
        {
            _eventCodeDic.Add(code, eventType);
            _eventTypeDic.Add(eventType, code);
        }

        public EVENT_CODES GetEventByCode(byte code)
        {
            if (_eventCodeDic.ContainsKey(code))
            {
                return _eventCodeDic[code];
            }
            return EVENT_CODES.INVALID;
        }
        
        public byte GetEventCode(EVENT_CODES eventType)
        {
            return _eventTypeDic[eventType];
        }
    }

    public enum EVENT_CODES
    {
        INVALID,
        SET_COLOR,
        PLAYER_READY,
        PRESS_BUTTON,
        PLAYER_JOINED,
        ROOM_IS_FILLED,
        ALL_PLAYERS_READY,
        GAME_STARTED,
        TURN_STARTED,
        TURN_FINISHED,
        TURN_RESET
    }
}