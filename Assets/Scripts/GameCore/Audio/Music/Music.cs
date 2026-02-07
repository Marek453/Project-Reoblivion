using System;
using UnityEngine;

namespace GameCore.Audio.Music
{
    [Serializable]
    public class Music
    {
        public MusicEventType eventType;
        public AudioClip clip;
    }
}
