using UnityEngine;

namespace Managers
{
    public sealed class MCamera : BaseManager
    {
        [SerializeField] private Camera _camera;
        
        public Transform GetCameraTransform => _camera.transform;
        public Camera GetCamera => _camera;
        
        protected override void Init()
        {
            base.Init();
        }

        protected override void Launch()
        {
            base.Launch();
        }

        protected override void Clear()
        {
            base.Clear();
        }
    }
}