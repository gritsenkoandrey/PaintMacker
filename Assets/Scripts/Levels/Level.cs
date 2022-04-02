using BaseMonoBehaviour;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Levels
{
    public sealed class Level : BaseComponent
    {
        [SerializeField] private CinemachineVirtualCamera[] _cameras;

        public CinemachineVirtualCamera[] Cameras => _cameras;

        [Button]
        public void GetCameras()
        {
            _cameras = GetComponentsInChildren<CinemachineVirtualCamera>();
        }
    }
}