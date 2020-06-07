namespace Sources.Photon
{
    public interface IGameUserInteractions
    {
        void SendButtonPressEvent();
        void SendGameSceneLoadedEvent();
        void SendCharacterIsDeadEvent();
    }
}
