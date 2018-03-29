using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Metadata
{
    public static class Tags
    {
        public const string COLLIDER = "Collider";
        public const string PLAYER = "Player";
    }
    
    public static class GameEvents
    {
        public const string UPDATE = "Collider";
    }
    
    public enum Layers
    {
        Default = 1 << 0,
        IgnoreRaycast = 1 << 2,
        UI = 1 << 5,
        Player = 1 << 8,
        MixedWorld = 1 << 9,
        RealWorld = 1 << 10,
        GhostWorld = 1 << 11
    }
}


