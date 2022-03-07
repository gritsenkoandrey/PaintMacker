using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "SettingsData", menuName = "Data/Settings", order = 0)]
    public sealed class SettingsData : ScriptableObject
    {
        [SerializeField] private float _timeToLose;
        [SerializeField] private float _timeToWin;

        [SerializeField] private float _countToWin;

        public float GetTimeToLose => _timeToLose;
        public float GetTimeToWin => _timeToWin;
        public float GetCountToWin => _countToWin;
    }
}