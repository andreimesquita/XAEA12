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
            public string ReadablePattern = default;
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
            PatternMapperSO patternMapperSo = Resources.Load<PatternMapperSO>("PatternMapperSO");
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

        [ContextMenu("Set Values")]
        public void SetValues()
        {
            string BLUE = "0001";
            string GREEN = "0010";
            string RED = "0100";
            string YELLOW = "1000";

            string[] a = {
                "1234",
                "1243",
                "1324",
                "1342",
                "1423",
                "1432",
                "2341",
                "2314",
                "2431",
                "2413",
                "2134",
                "2143",
                "3412",
                "3421",
                "3241",
                "3214",
                "3142",
                "3124",
                "4123",
                "4132",
                "4213",
                "4231",
                "4321",
                "4312"
            };

            for (int i = 0; i < _patterns.Length; i++)
            {
                switch (a[i].Substring(0, 1))
                {
                    case "1":
                        _patterns[i].Pattern = "0001" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "BLUE ";
                        break;
                    case "2":
                        _patterns[i].Pattern = "0010" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "GREEN ";
                        break;
                    case "3":
                        _patterns[i].Pattern = "0100" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "RED ";
                        break;
                    case "4":
                        _patterns[i].Pattern = "1000" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "YELLOW ";
                        break;
                }
                switch (a[i].Substring(1, 1))
                {
                    case "1":
                        _patterns[i].Pattern = "0001" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "BLUE ";
                        break;
                    case "2":
                        _patterns[i].Pattern = "0010" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "GREEN ";
                        break;
                    case "3":
                        _patterns[i].Pattern = "0100" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "RED ";
                        break;
                    case "4":
                        _patterns[i].Pattern = "1000" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "YELLOW ";
                        break;
                }
                switch (a[i].Substring(2, 1))
                {
                    case "1":
                        _patterns[i].Pattern = "0001" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "BLUE ";
                        break;
                    case "2":
                        _patterns[i].Pattern = "0010" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "GREEN ";
                        break;
                    case "3":
                        _patterns[i].Pattern = "0100" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "RED ";
                        break;
                    case "4":
                        _patterns[i].Pattern = "1000" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "YELLOW ";
                        break;
                }
                switch (a[i].Substring(3, 1))
                {
                    case "1":
                        _patterns[i].Pattern = "0001" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "BLUE ";
                        break;
                    case "2":
                        _patterns[i].Pattern = "0010" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "GREEN ";
                        break;
                    case "3":
                        _patterns[i].Pattern = "0100" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "RED ";
                        break;
                    case "4":
                        _patterns[i].Pattern = "1000" + _patterns[i].Pattern;
                        _patterns[i].ReadablePattern += "YELLOW ";
                        break;
                }
            }
        }
    }
}
