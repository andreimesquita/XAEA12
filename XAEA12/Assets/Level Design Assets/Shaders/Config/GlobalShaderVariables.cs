using UnityEngine;

namespace Config
{
    public static class GlobalShaderVariables
    {
        private const string _nameCharacterWorldPos = "CHARACTER_WORLD_POS";
        private const string _nameMinDisplacementDistance = "MIN_DISPLACEMENT_DISTANCE";
        private const string _nameCurvature = "CURVATURE";
        
        public static readonly int CharacterWorldPos = Shader.PropertyToID(_nameCharacterWorldPos);
        public static readonly int MinDisplacementDistance = Shader.PropertyToID(_nameMinDisplacementDistance);
        public static readonly int Curvature = Shader.PropertyToID(_nameCurvature);
    }
}
