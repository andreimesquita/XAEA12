using UnityEngine;

namespace Sources.Common.Pattern
{
    public sealed class PatternMapperSO : ScriptableObject
    {
        private Dictionary<int, string> _triggerByPattern;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Load()
        {
            //TODO initialize singleton instance
        }

        public bool IsValidPattern(int pattern)
        {
            return _triggerByPattern.ContainsKey(pattern);
        }
    }
}
