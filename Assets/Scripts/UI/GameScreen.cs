using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class GameScreen : BaseScreen
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private CanvasGroup _canvasGroup;

        private int _current;

        private readonly CompositeDisposable _timerDisposable = new CompositeDisposable();

        protected override void Subscribe()
        {
            base.Subscribe();
            
            Init();

            _restartButton
                .OnClickAsObservable()
                .First()
                .Subscribe(_ =>
                {
                    _restartButton.transform
                        .DOScale(Vector3.one * 0.5f, 0.5f)
                        .SetEase(Ease.InBack)
                        .OnComplete(() =>
                        {
                            Game.LaunchRound.Execute(false);

                            SetScaleRestartButton(Vector3.one);
                        });
                })
                .AddTo(screenDisposable);

            World.Progress
                .Subscribe(next =>
                {
                    _timerDisposable.Clear();

                    int count = next - _current;
                    float time = 1f / count;
                    
                    if (count == 0) return;

                    Observable
                        .Interval(TimeSpan.FromSeconds(time))
                        .Subscribe(_ =>
                        {
                            SetProgressText(_current++);

                            if (_current >= 100)
                            {
                                SetFadeProgress(0f);
                            }

                            if (count == 0)
                            {
                                _timerDisposable.Clear();
                            }

                            count--;
                        })
                        .AddTo(_timerDisposable)
                        .AddTo(screenDisposable);
                })
                .AddTo(screenDisposable);
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            
            screenDisposable.Clear();
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
            _current = 0;
            _canvasGroup.alpha = 0f;
            _restartButton.transform.localScale = Vector3.zero;

            SetProgressText(_current);
            SetFadeProgress(1f);
            SetScaleRestartButton(Vector3.one);
        }

        private void SetProgressText(int value)
        {
            _progressText.text = $"Painted:\n{value} %";
        }

        private void SetFadeProgress(float value)
        {
            _canvasGroup
                .DOFade(value, 1f)
                .SetEase(Ease.Linear);
        }

        private void SetScaleRestartButton(Vector3 scale)
        {
            _restartButton.transform
                .DOScale(scale, 0.5f)
                .SetEase(Ease.OutBack);
        }
    }
}