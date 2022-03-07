using UnityEngine;

namespace Managers
{
    public sealed class MLight : BaseManager
    {
        [SerializeField] private Light _light;
        
        public Light GetLight => _light;
        
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