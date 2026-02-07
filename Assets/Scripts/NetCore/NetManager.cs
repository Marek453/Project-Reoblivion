using Mirror;
using GameCore.Audio.Music;
using UnityEngine;

namespace NetCore
{
    public class NetManager : NetworkManager
    {
        public static NetManager Instance;

        private void Awake()
        {
            Instance = this;
        }
        public override void OnClientConnect()
        {
            base.OnClientConnect();
            MusicManager.Instance.Stop();
        }

        public override void OnStartClient()
        {
            ConnectionUI.GetConnectionUI.Connect();
            MusicManager.Instance.PlayMusic(MusicEventType.Intense, true, false, 38f);
            base.OnStartClient();
        }

        public override void OnClientError(TransportError error, string reason)
        {
            MusicManager.Instance.PlayMusic(MusicEventType.Loop, true);
            ConnectionUI.GetConnectionUI.Disconnect(reason);
            base.OnClientError(error, reason);
        }

       
    }
}
