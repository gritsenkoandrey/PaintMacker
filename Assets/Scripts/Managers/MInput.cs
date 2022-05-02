using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Managers
{
    public sealed class MInput : BaseManager
    {
        [SerializeField] private float _dirThreshold = 0.75f;
        [SerializeField] private float _minDistance = 0.1f;

        private Vector3 _inputStart;
        private Vector3 _inputEnd;

        [HideInPrefabs] public readonly ReactiveProperty<bool> IsEnable = new ReactiveProperty<bool>();

        public readonly ReactiveCommand OnSwipeUp = new ReactiveCommand();
        public readonly ReactiveCommand OnSwipeDown = new ReactiveCommand();
        public readonly ReactiveCommand OnSwipeLeft = new ReactiveCommand();
        public readonly ReactiveCommand OnSwipeRight = new ReactiveCommand();
        
        private readonly CompositeDisposable _inputDisposable = new CompositeDisposable();
        
        protected override void Enable()
        {
            base.Enable();
            
            IsEnable
                .Subscribe(value =>
                {
                    if (value)
                    { 
                        Observable
                            .EveryUpdate()
                            .Subscribe(_ =>
                            {
                                UpdateInput();
                            })
                            .AddTo(_inputDisposable);
                    }
                    else
                    {
                        _inputDisposable.Clear();
                    }
                })
                .AddTo(LifeTimeDisposable);
        }

        protected override void Launch()
        {
            base.Launch();
        }

        protected override void Disable()
        {
            base.Disable();
            
            _inputDisposable.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateInput()
        {
            
#if UNITY_EDITOR
            
            Vector3 mousePosition = Input.mousePosition;
        
            if (Input.GetMouseButtonDown(0))
            {
                _inputStart = mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _inputEnd = mousePosition;
                
                SwipeDetected();
            }
#endif

#if UNITY_ANDROID

            if (Input.touchCount <= 0) return;
            
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _inputStart = touchPosition;
                    break;
                case TouchPhase.Ended:
                    _inputEnd = touchPosition;
                    SwipeDetected();
                    break;
                case TouchPhase.Canceled:
                    _inputEnd = touchPosition;
                    SwipeDetected();
                    break;
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SwipeDetected()
        {
            if ((_inputStart - _inputEnd).sqrMagnitude > _minDistance)
            {
                Vector3 dir = (_inputEnd - _inputStart).normalized;

                if (Vector2.Dot(Vector2.up, dir) > _dirThreshold)
                {
                    OnSwipeUp.Execute();
                }
                else if (Vector2.Dot(Vector2.down, dir) > _dirThreshold)
                {
                    OnSwipeDown.Execute();
                }
                else if (Vector2.Dot(Vector2.right, dir) > _dirThreshold)
                {
                    OnSwipeRight.Execute();
                }
                else if (Vector2.Dot(Vector2.left, dir) > _dirThreshold)
                {
                    OnSwipeLeft.Execute();
                }
            }
        }
    }
}