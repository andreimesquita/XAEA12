using System.Collections;
using UnityEngine;

namespace Sources.Views
{
    public delegate void Callback();
    
    public sealed class EventProxy : MonoBehaviour
    {
        private static EventProxy _instance;
        public static EventProxy Instance
        {
            get
            {
                if (_isInstantiated) return _instance;
                var globalComponents = new GameObject("globalComponents");
                DontDestroyOnLoad(globalComponents);
                _instance = globalComponents.AddComponent<EventProxy>();
                return _instance;
            }
        }
        
        public readonly Observer OnUpdate = new Observer();
        public readonly Observer OnLateUpdate = new Observer();
        public readonly Observer OnFixedUpdate = new Observer();

        private static bool _isInstantiated;
        
        private void Update()
        {
            OnUpdate.Notify();
        }
        
        private void LateUpdate()
        {
            OnLateUpdate.Notify();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate.Notify();
        }

        public void InvokeLater(YieldInstruction instruction, Callback callback)
        {
            IEnumerator coroutine = RunInstructionAndInvoke();
            StartCoroutine(coroutine);
            
            IEnumerator RunInstructionAndInvoke()
            {
                yield return instruction;
                callback?.Invoke();
            }
        }
    }
}
