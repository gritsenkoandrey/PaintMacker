using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public sealed class LoseScreen : BaseScreen
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        protected override void Subscribe()
        {
            base.Subscribe();
            
            Init();

            _restartButton
                .OnClickAsObservable()
                .First()
                .Subscribe(_ =>
                {
                    Game.LaunchRound.Execute(false);
                })
                .AddTo(screenDisposable);
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            
            screenDisposable.Clear();
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
            _restartButton.interactable = false;
            _restartButton.transform.localScale = Vector3.zero;
            _canvasGroup.transform.localScale = Vector3.one * 2f;
            _canvasGroup.alpha = 0f;
            
            sequence = sequence.RefreshSequence();

            sequence
                .Append(_canvasGroup.transform
                    .DOScale(Vector3.one, 0.15f)
                    .SetEase(Ease.Linear))
                .Join(_canvasGroup
                    .DOFade(1f, 0.1f)
                    .SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    _restartButton.transform
                        .DOScale(Vector3.one, 0.5f)
                        .SetEase(Ease.OutBack)
                        .OnComplete(() => _restartButton.interactable = true);
                });
        }
    }
}