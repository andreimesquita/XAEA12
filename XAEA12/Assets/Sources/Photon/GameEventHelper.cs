using System.Collections.Generic;

namespace Sources.Photon
{
    public class GameEventHelper
    {
        private Dictionary<byte, EVENT_CODES> _eventCodeDic;
        private Dictionary<EVENT_CODES, byte> _eventTypeDic;

        public GameEventHelper()
        {
            _eventCodeDic = new Dictionary<byte, EVENT_CODES>();
            _eventTypeDic = new Dictionary<EVENT_CODES, byte>();
            _eventCodeDic.Add(1, EVENT_CODES.SET_COLOR);
            _eventTypeDic.Add(EVENT_CODES.SET_COLOR, 1);
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
        SET_COLOR        
    }
}