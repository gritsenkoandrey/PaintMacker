using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public enum AnimationType
    {
        Idle    = 0,
        Run     = 1,
        Victory = 2,
        Death   = 4
    }
    
    [System.Serializable]
    public struct Pixel
    {
        public int index;
        public int x;
        public int y;
    }
    
    [System.Serializable]
    public enum GroundType : byte
    {
        Ground     = 0,
        Frame      = 1,
        Forward    = 2,
        Deactivate = 4,
        Walking    = 6
    }

    [System.Serializable]
    public struct Materials
    {
        public Material frame;
        public Material painted;
        public Material unpainted;
    }
}