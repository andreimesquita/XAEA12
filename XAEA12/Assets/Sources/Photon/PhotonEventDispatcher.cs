using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Object = System.Object;

namespace Sources.Photon
{
    public sealed class PhotonEventDispatcher
    {
        private readonly GameEventHelper _eventHelper;

        public PhotonEventDispatcher(GameEventHelper eventHelper)
        {
            _eventHelper = eventHelper;
        }
        
        public void SendEventToMaster(EVENT_CODES eventCode, Object content)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
            SendEvent(eventCode, content, raiseEventOptions);
        }

        public void SendEventToPlayer(EVENT_CODES eventCode, Object content, int playerActor)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new[] {playerActor}};
            SendEvent(eventCode, content, raiseEventOptions);
        }

        public void SendEventToServer(EVENT_CODES eventCode, Object content) 
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendEvent(eventCode, content, raiseEventOptions);
        }

        private void SendEvent(EVENT_CODES eventCode, Object content, RaiseEventOptions raiseEventOptions)
        {
            SendOptions sendOptions = new SendOptions { Reliability = true};
            //Debug.Log($"Sending Event: {_eventHelper.GetEventCode(eventCode)}, {content}");
            PhotonNetwork.RaiseEvent(_eventHelper.GetEventCode(eventCode), content, raiseEventOptions, sendOptions);
        }
    }
}
