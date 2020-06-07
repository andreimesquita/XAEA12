namespace Sources.Photon
{
    public class GameEventHelper
    {
        // Button Colors
        public const byte Blue   = 1;
        public const byte Green  = 2;
        public const byte Red    = 4;
        public const byte Yellow = 8;
        
        public byte GetEventCode(EVENT_CODES eventType)
        {
            return (byte) eventType;
        }
    }
}
