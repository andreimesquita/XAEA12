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
            public string Pattern = default;
        }

        [Serializable]
        private class ColorPatternEntry
        {
            public string ColorPattern;
            public Color TargetColor;
        }

        private static PatternMapperSO _instance;
        public static PatternMapperSO Instance => _instance;
        
        [SerializeField]
        private PatternEntry[] _patterns = default;

        [SerializeField]
        private ColorPatternEntry[] _colorPatterns = default;

        private Dictionary<int, string> _triggerByPattern = default;
        private Dictionary<int, Color> _colorsByPattern = default;

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
                    int bitPattern = Convert.ToInt32(patternEntry.Pattern, 2);
                    _triggerByPattern[bitPattern] = patternEntry.AnimationTrigger;
                }
            }
            
            _colorsByPattern = new Dictionary<int, Color>();
            if (_colorPatterns != null)
            {
                foreach (ColorPatternEntry colorPattern in _colorPatterns)
                {
                    int bitPattern = Convert.ToInt32(colorPattern.ColorPattern, 2);
                    _colorsByPattern[bitPattern] = colorPattern.TargetColor;
                }
            }
        }

        public bool IsValidPattern(int pattern)
        {
            return _triggerByPattern.ContainsKey(pattern);
        }

        public Color GetColorByPattern(byte pattern)
        {
            if (_colorsByPattern.ContainsKey(pattern))
            {
                return _colorsByPattern[pattern];
            }
            return Color.magenta;
        }
    }
}
