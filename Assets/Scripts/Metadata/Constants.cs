using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Metadata
{
    public static class Tags
    {
        public static readonly string COLLIDER = "Collider";
        public static readonly string PLAYER = "Player";
    }
    
    public static class GameEvents
    {
        public static readonly string UPDATE = "Collider";
    }
    
    public static class ResourcesFolder
    {
        public static readonly string GameData = "GameData";
    }

    public static class SaveKeys
    {
        public static readonly string Zones = "zone_{0}";
        public static readonly string LastSave = "LastSave";
        public static readonly string LastSaveScene = "LastSave_{0}";
        public static readonly string UsedSave = "save_{0}";
        public static readonly string ConsoleSave = "console_{0}";
        public static readonly string CharacterHealth = "mainchar_health";
        public static readonly string CharacterSpirit = "mainchar_spirit";
        public static readonly string CharacterDash = "mainchar_dash";
    }

    public static class FormatedLog
    {
        public static readonly string Save = "[SAVE] - {0}";
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


