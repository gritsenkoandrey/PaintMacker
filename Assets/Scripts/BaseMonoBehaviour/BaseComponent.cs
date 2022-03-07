using UniRx;
using UnityEngine;

namespace BaseMonoBehaviour
{
    public abstract class BaseComponent : MonoBehaviour
    {
        protected readonly CompositeDisposable lifetimeDisposable = new CompositeDisposable();
        
        private void OnEnable()
        {
            Enable();
        }

        private void OnDisable()
        {
            Disable();
        }

        private void Start()
        {
            Init();
        }

        protected virtual void Init() {}
        protected virtual void Enable() {}

        protected virtual void Disable()
        {
            lifetimeDisposable.Clear();
        }
    }
}