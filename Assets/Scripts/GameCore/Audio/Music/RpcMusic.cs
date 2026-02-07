using System;
using GameCore.Player.Class;
using UnityEngine;

namespace GameCore.Audio.Music
{
    [Serializable]
    public class RpcMusic
    {
       public RoleType roleTypeMusic;
       public RpcTypeMusic[] rpcTypeMusics;
    }
}