using UnityEngine;

namespace Environment.Traps
{
    [System.Serializable]
    public struct TrapSettings
    {
        public TrapType type;
        public Vector3 vector;
        public float duration;
        public float delay;
    }
}