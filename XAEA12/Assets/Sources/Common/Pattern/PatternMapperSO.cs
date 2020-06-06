using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Common.Pattern
{
    [CreateAssetMenu(menuName = "XAEA12/Create PatternMapperSO", fileName = "PatternMapperSO")]
    public sealed class PatternMapperSO : ScriptableObject
    {
        [Serializable]
        private class PatternEntry
        {
            public string AnimationTrigger = default;
            //TODO(andrei) create a custom drawer for this
            public int Pattern = default;
        }

        private static PatternMapperSO _instance;
        public static PatternMapperSO Instance => _instance;

        [SerializeField]
        private PatternEntry[] _patterns = default;

        private Dictionary<int, string> _triggerByPattern = default;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Load()
        {
            PatternMapperSO patternMapperSo =  Resources.Load<PatternMapperSO>("PatternMapperSO");
            _instance = patternMapperSo;
            patternMapperSo.TranslateToDictionary();
        }
        
        private void TranslateToDictionary()
        {
            _triggerByPattern = new Dictionary<int, string>();
            if (_patterns != null)
            {
                foreach (PatternEntry patternEntry in _patterns)
                {
                    _triggerByPattern[patternEntry.Pattern] = patternEntry.AnimationTrigger;
                }
            }
        }

        public bool IsValidPattern(int pattern)
        {
            return _triggerByPattern.ContainsKey(pattern);
        }
    }
}
