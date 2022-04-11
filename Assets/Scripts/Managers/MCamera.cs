using UnityEngine;

namespace Managers
{
    public sealed class MCamera : BaseManager
    {
        [SerializeField] private Camera _camera;
        
        public Transform GetCameraTransform => _camera.transform;
        public Camera GetCamera => _camera;
        
        protected override void Enable()
        {
            base.Enable();
        }

        protected override void Launch()
        {
            base.Launch();
        }

        protected override void Disable()
        {
            base.Disable();
        }
    }
}