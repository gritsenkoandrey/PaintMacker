using BaseMonoBehaviour;

namespace Managers
{
    public abstract class BaseManager : BaseComponent
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            
            Enable();
        }

        protected override void Start()
        {
            base.Start();
            
            Launch();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            Disable();
        }

        protected virtual void Enable() {}
        protected virtual void Launch() {}
        protected virtual void Disable()
        {
            LifeTimeDisposable.Clear();
        }
    }
}