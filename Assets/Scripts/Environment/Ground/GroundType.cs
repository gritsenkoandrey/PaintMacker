namespace Environment.Ground
{
    [System.Serializable]
    public enum GroundType : byte
    {
        Ground     = 0,
        Frame      = 1,
        Forward    = 2,
        Deactivate = 4,
        Walking    = 6
    }
}