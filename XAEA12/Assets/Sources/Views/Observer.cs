namespace Sources.Views
{
    public sealed class Observer
    {
        private Callback _listeners;
        
        public void AddListener(Callback callback)
        {
            _listeners += callback;
        }
        
        public void RemoveListener(Callback callback)
        {
            _listeners -= callback;
        }

        public void Notify()
        {
            _listeners?.Invoke();
        }
    }
}
