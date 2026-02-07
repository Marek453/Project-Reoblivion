using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using GameCore.Console;
using System;
using GameCore.Player.Class;
using System.Linq;

namespace GameCore.Audio.Music
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance;
        public List<RpcMusic> rpcMusics;
        public List<Music> musics;
        public RpcType rpcType;
        public MusicEventType currentMusicType;
        public AudioSource Main;
        public AudioSource Second;
        DeveloperConsole console;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            PlayMusic(MusicEventType.Startup,isFadeOut: false);
            console = DeveloperConsole.singleton;
            if (console != null)
            {
                console.RegisterCommand("set_music", SetMusic);
            }
        }

        public void PlayRpcMusic(RoleType roleType,RpcType type, bool isLoop = false, bool isOneShot = false, float duraction = 0, float maxVolune = 1, bool isFadeOut = true)
        {
            if(roleType == RoleType.None)
            {
                PlayMusic(currentMusicType,true);
                return;
            }
            RpcMusic music = rpcMusics.Find(mc => mc.roleTypeMusic == roleType);
            RpcTypeMusic rpcTypeMusic = music.rpcTypeMusics.ToList().Find(mc => mc.rpcTypeMusic == type);

            if (Main.volume == maxVolune)
            {
                Change(Second, Main, rpcTypeMusic.clip, isLoop, isOneShot, duraction, maxVolune, isFadeOut);
            }
            else if (Second.volume == maxVolune)
            {
                Change(Main, Second, rpcTypeMusic.clip, isLoop, isOneShot, duraction, maxVolune, isFadeOut);
            }
            else
            {
                Change(Main, Second, rpcTypeMusic.clip, isLoop, isOneShot, duraction, maxVolune, isFadeOut);
            }
            currentMusicType = MusicEventType.None;
            rpcType = rpcTypeMusic.rpcTypeMusic;
        }

        private void Update()
        {
            if(currentMusicType == MusicEventType.Startup && !Main.isPlaying)
            {
                PlayMusic(MusicEventType.Loop, true, isFadeOut: false);
            }
        }

        public void Stop()
        {
            Tween.Custom(Main.volume, 0, 2, v => Main.volume = v);
            Tween.Custom(Second.volume, 0, 2, v => Second.volume = v);
        }

        private void Change(AudioSource source,AudioSource oldSource,AudioClip clip, bool isLoop, bool isOneShot , float duraction , float maxVolune, bool isFadeOut)
        {
            source.loop = isLoop;
            source.Stop();
            if (isOneShot)
            {
                source.PlayOneShot(clip);
            }
            else
            {
                source.clip = clip;
                source.Play();
                source.time = duraction;

            }
            if (isFadeOut)
            {
                Tween.Custom(oldSource.volume, 0, 2, v => oldSource.volume = v);
                Tween.Custom(source.volume, maxVolune, 2, v => source.volume = v);
            }
            else
            {
                oldSource.volume = 0;
                source.volume = 1;
            }
        }

        public void SetMusic(string[] args)
        {
            if (args.Length > 0 && Enum.TryParse<MusicEventType>(args[0],true, out MusicEventType amount))
            {
                bool.TryParse(args[1], out var isloop);
                bool.TryParse(args[2], out var isOneShot);
                float.TryParse(args[3], out var duraction);
                float.TryParse(args[4], out var maxVolune);
                bool.TryParse(args[5], out var isFadeOut);
                PlayMusic(amount, isloop, isOneShot, duraction, maxVolune, isFadeOut);
                Debug.Log($"Loading music: {args[0]}...");
            }
            else
            {
                Debug.LogError("Usage: set_music <MusicEventType> <isLoop> <isOneShot> <duraction> <maxVolune> <isFadeOut>");
            }
        }



        public void PlayMusic(MusicEventType type, bool isLoop = false, bool isOneShot = false, float duraction = 0, float maxVolune = 1, bool isFadeOut = true)
        {
            Music music = musics.Find(mc => mc.eventType == type);
            if(Main.volume == maxVolune)
            {
                Change(Second, Main, music.clip, isLoop, isOneShot, duraction, maxVolune, isFadeOut);
            }
            else if (Second.volume == maxVolune)
            {
                Change(Main, Second, music.clip, isLoop, isOneShot, duraction, maxVolune, isFadeOut);
            }
            else
            {
                Change(Main, Second, music.clip, isLoop, isOneShot, duraction, maxVolune, isFadeOut);
            }
            currentMusicType = type;
        }
    }
}
