using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "GlobalShaderVariables", menuName = "Create GlobalShaderVariables here")]
    public sealed class GlobalShaderManager : ScriptableObject
    {
        private static GlobalShaderManager _instance;

        [Header("Curved World")]
        [Range(0f, 0.5f)]
        [SerializeField]
        private float _curvature = 0.0007f;
        [Range(0f, 1000f)]
        [SerializeField]
        private float _minDisplacementDistance = 0f;

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeInitialized()
        {
            _instance = Resources.Load<GlobalShaderManager>("GlobalShaderVariables");
            _instance.UpdateCurvatureValue();
            _instance.UpdateMinDisplacementDistanceValue();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += StateChange;
#endif
        }
        
#if UNITY_EDITOR
        static void StateChange(UnityEditor.PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == UnityEditor.PlayModeStateChange.ExitingPlayMode
                || playModeStateChange == UnityEditor.PlayModeStateChange.EnteredEditMode)
            {
                UnityEditor.EditorApplication.playModeStateChanged -= StateChange;
                DisableCurvatureValue();
            }
        }
#endif

        private void OnDisable()
        {
            DisableCurvatureValue();
        }

        [ContextMenu("Update Curvature")]
        private void UpdateCurvatureValue()
        {
            Shader.SetGlobalFloat(GlobalShaderVariables.Curvature, _curvature);
        }

        [ContextMenu("Update Character Position")]
        private void UpdateCharacterPosition()
        {
            SetCharacterWorldPos(Vector3.zero);
        }
        
        [ContextMenu("Disable Curvature")]
        private void EditorDisableCurvatureValue()
        {
            DisableCurvatureValue();
        }
        
        private static void DisableCurvatureValue()
        {
            Shader.SetGlobalFloat(GlobalShaderVariables.Curvature, 0f);
        }
        
        [ContextMenu("Update Min Displacement Distance")]
        private void UpdateMinDisplacementDistanceValue()
        {
            Shader.SetGlobalFloat(GlobalShaderVariables.MinDisplacementDistance, _minDisplacementDistance);
        }

        public static void SetCharacterWorldPos(Vector3 worldPos)
        {
            var vector = new Vector4(worldPos.x, worldPos.y, worldPos.z, 0);
            Shader.SetGlobalVector(GlobalShaderVariables.CharacterWorldPos, vector);
        }
    }
}
