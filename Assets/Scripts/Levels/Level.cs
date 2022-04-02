using Cinemachine;
using Environment.Ground;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Levels
{
    public sealed class Level : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera[] _cameras;
        
        [SerializeField] private Materials _materials;

        public CinemachineVirtualCamera[] Cameras => _cameras;
        public Materials Materials => _materials;

        [Button]
        public void GetCameras()
        {
            _cameras = GetComponentsInChildren<CinemachineVirtualCamera>();
        }
    }
}