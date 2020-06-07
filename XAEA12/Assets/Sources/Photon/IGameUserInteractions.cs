namespace Sources.Photon
{
    public interface IGameUserInteractions
    {
        void SendButtonPressEvent();
        void SendGameSceneReadyEvent();
        void SendCharacterIsDeadEvent();
    }
}
