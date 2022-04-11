using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public sealed class WinScreen : BaseScreen
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        protected override void Subscribe()
        {
            base.Subscribe();
            
            Init();

            _nextButton
                .OnClickAsObservable()
                .First()
                .Subscribe(_ =>
                {
                    Game.LaunchRound.Execute(true);
                })
                .AddTo(LifeTimeDisposable);
        }

        protected override void Unsubscribe()
        {
            LifeTimeDisposable.Clear();
        }

        protected override void Initialize()
        {
            base.Initialize();
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
            _nextButton.interactable = false;
            _nextButton.transform.localScale = Vector3.zero;
            _canvasGroup.transform.localScale = Vector3.one * 2f;
            _canvasGroup.alpha = 0f;
            
            Sequence = Sequence.RefreshSequence();

            Sequence
                .Append(_canvasGroup.transform
                    .DOScale(Vector3.one, 0.15f)
                    .SetEase(Ease.Linear))
                .Join(_canvasGroup
                    .DOFade(1f, 0.1f)
                    .SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    _nextButton.transform
                        .DOScale(Vector3.one, 0.5f)
                        .SetEase(Ease.OutBack)
                        .OnComplete(() => _nextButton.interactable = true);
                });
        }
    }
}