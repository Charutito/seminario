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
