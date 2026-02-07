using GameCore.Audio.Music;
using UnityEngine;

namespace GameCore.UI
{
    public class CreditsEnd : MonoBehaviour
    {
        public void Stop()
        {
            MusicManager.Instance.PlayMusic(MusicEventType.Loop,true,false,0);
        }
    }
}
