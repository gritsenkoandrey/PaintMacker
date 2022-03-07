using System;
using UniRx;
using UnityEngine;

namespace Managers
{
    public abstract class BaseManager : MonoBehaviour, IDisposable
    {
        protected readonly CompositeDisposable ManagerDisposable = new CompositeDisposable();

        protected virtual void Init() {}
        protected virtual void Launch() {}
        protected virtual void Clear()
        {
            ManagerDisposable?.Dispose();
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            Launch();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}