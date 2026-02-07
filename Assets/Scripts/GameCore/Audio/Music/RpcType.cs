using UnityEngine;

namespace GameCore.Audio.Music
{
    public enum RpcType
    {
        None = -1,
        Idle,
        IdleTarget,
        NonTarget,
        Target,
        RangeEndTarget,
        RangeEnd,
        See,
        InDistance,
        OutDistance
    }
}