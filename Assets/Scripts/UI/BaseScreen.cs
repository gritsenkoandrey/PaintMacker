using DG.Tweening;
using Managers;
using UniRx;
using UnityEngine;
using Utils;

namespace UI
{
    public abstract class BaseScreen : MonoBehaviour
    {
        protected MGame Game { get; private set; }
        protected MWorld World { get; private set; }
        
        protected Tweener tween;
        protected Sequence sequence;

        protected readonly CompositeDisposable screenDisposable = new CompositeDisposable();

        private void Awake()
        {
            Game = Manager.Resolve<MGame>();
            World = Manager.Resolve<MWorld>();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            screenDisposable.Clear();
            tween.KillTween();
        }

        protected virtual void Initialize() {}
        protected virtual void Subscribe() {}
        protected virtual void Unsubscribe() {}
        
        public abstract void Show();
        public abstract void Hide();
    }
}