using BaseMonoBehaviour;
using DG.Tweening;
using Managers;

namespace UI
{
    public abstract class BaseScreen : BaseComponent
    {
        protected MGame Game { get; private set; }
        protected MWorld World { get; private set; }
        protected Tweener Tween { get; set; }
        protected Sequence Sequence { get; set; }

        protected override void Awake()
        {
            Game = Manager.Resolve<MGame>();
            World = Manager.Resolve<MWorld>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            Subscribe();
        }

        protected override void Start()
        {
            base.Start();
            
            Initialize();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            Unsubscribe();
        }

        protected virtual void Initialize() {}
        protected virtual void Subscribe() {}
        protected virtual void Unsubscribe() {}
        
        public abstract void Show();
        public abstract void Hide();
    }
}