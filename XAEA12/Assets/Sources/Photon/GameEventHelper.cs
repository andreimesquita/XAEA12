using System.Collections.Generic;

namespace Sources.Photon
{
    public class GameEventHelper
    {
        private readonly Dictionary<byte, EVENT_CODES> _eventCodeDic;
        private readonly Dictionary<EVENT_CODES, byte> _eventTypeDic;

        // Button Colors
        public const byte Blue   = 1;
        public const byte Green  = Blue << 1;
        public const byte Red    = Green << 1;
        public const byte Yellow = Red << 1;

        public GameEventHelper()
        {
            _eventCodeDic = new Dictionary<byte, EVENT_CODES>();
            _eventTypeDic = new Dictionary<EVENT_CODES, byte>();
            
            addEventToMap(EVENT_CODES.INVALID);
            addEventToMap(EVENT_CODES.SET_CLIENT_COLOR);
            addEventToMap(EVENT_CODES.NOTIFY_CLIENT_READY);
            addEventToMap(EVENT_CODES.CLIENT_PRESSED_BUTTON);
            addEventToMap(EVENT_CODES.ROOM_IS_FILLED);
            addEventToMap(EVENT_CODES.ALL_PLAYERS_READY);
            addEventToMap(EVENT_CODES.GAME_STARTED);
            addEventToMap(EVENT_CODES.TURN_STARTED);
            addEventToMap(EVENT_CODES.TURN_FINISHED);
            addEventToMap(EVENT_CODES.TURN_RESET);
        }

        private void addEventToMap(EVENT_CODES eventType)
        {
            byte code = (byte) eventType;
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
}