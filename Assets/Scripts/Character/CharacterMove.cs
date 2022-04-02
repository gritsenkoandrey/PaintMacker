using System.Runtime.CompilerServices;
using Managers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

namespace Character
{
    public sealed class CharacterMove : ICharacter
    {
        private readonly CharacterModel _model;
        
        private readonly MInput _input;
        private readonly MConfig _config;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public CharacterMove(CharacterModel model)
        {
            _model = model;
            
            _input = Manager.Resolve<MInput>();
            _config = Manager.Resolve<MConfig>();
        }

        public void Register()
        {
            _input.OnSwipeUp
                .Subscribe(_ => ForwardTo(Vector3.forward))
                .AddTo(_disposable);

            _input.OnSwipeDown
                .Subscribe(_ => ForwardTo(Vector3.back))
                .AddTo(_disposable);

            _input.OnSwipeLeft
                .Subscribe(_ => ForwardTo(Vector3.left))
                .AddTo(_disposable);

            _input.OnSwipeRight
                .Subscribe(_ => ForwardTo(Vector3.right))
                .AddTo(_disposable);

            _model.CharacterController
                .UpdateAsObservable()
                .Where(_ => _model.IsMove.Value)
                .Subscribe(_ => Move())
                .AddTo(_model.CharacterDisposable)
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        private void ForwardTo(Vector3 vector)
        {
            _model.Transform.forward = vector;
            
            CanMove();
        }

        private void CanMove()
        {
            if (!_model.IsMove.Value && Physics.Raycast(_model.RayForward, 1f, Layers.GetWalking))
            {
                _model.IsMove.SetValueAndForceNotify(true);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Move()
        {
            if (Physics.Raycast(_model.RayForward, 1f, Layers.GetWalking))
            {
                _model.CharacterController
                    .Move(_model.Transform.forward * _config.CharacterData.Speed * Time.deltaTime);
            }
            else
            {
                _model.IsMove.SetValueAndForceNotify(false);
            }
        }
    }
}