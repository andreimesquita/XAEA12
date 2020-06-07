namespace Sources.Photon
{
    public class GameEventHelper
    {
        // Button Colors
        public const byte Blue   = 1;
        public const byte Green  = Blue << 1;
        public const byte Red    = Green << 1;
        public const byte Yellow = Red << 1;
        
        public byte GetEventCode(EVENT_CODES eventType)
        {
            return (byte) eventType;
        }
    }
}
