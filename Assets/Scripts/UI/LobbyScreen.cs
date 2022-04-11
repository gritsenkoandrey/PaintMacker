using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class LobbyScreen : BaseScreen
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private TextMeshProUGUI _text;

        protected override void Initialize()
        {
            base.Initialize();
            
            Tween = _text
                .DOScale(1.25f, 0.5f)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            
            Tween.Play();

            Init();

            _startButton
                .OnClickAsObservable()
                .First()
                .Subscribe(_ =>
                {
                    Game.OnRoundStart.Execute();
                })
                .AddTo(LifeTimeDisposable);
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            
            Tween.Pause();

            LifeTimeDisposable.Clear();
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Init()
        {
            _text.alpha = 0f;
            _text.DOFade(1f, 0f).SetDelay(0.25f);
        }
    }
}