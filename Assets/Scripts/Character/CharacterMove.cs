using Managers;
using UniRx;
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
            float speed = _config.CharacterData.Speed;
            
            _input.OnSwipeUp
                .Subscribe(_ =>
                {
                    Move(Vector3.forward);
                })
                .AddTo(_disposable);

            _input.OnSwipeDown
                .Subscribe(_ =>
                {
                    Move(Vector3.back);
                })
                .AddTo(_disposable);

            _input.OnSwipeLeft
                .Subscribe(_ =>
                {
                    Move(Vector3.left);
                })
                .AddTo(_disposable);

            _input.OnSwipeRight
                .Subscribe(_ =>
                {
                    Move(Vector3.right);
                })
                .AddTo(_disposable);

            Observable
                .EveryUpdate()
                .Where(_ => _model.IsMove.Value)
                .Subscribe(_ =>
                {
                    Ray ray = new Ray { origin = _model.Forward.position, direction = Vector3.down };

                    if (Physics.Raycast(ray, 1f, Layers.GetWalking))
                    {
                        _model.CharacterController
                            .Move(_model.CharacterController.transform.forward * speed * Time.deltaTime);
                    }
                    else
                    {
                        _model.IsMove.SetValueAndForceNotify(false);
                    }
                })
                .AddTo(_model.CharacterDisposable)
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        private void Move(Vector3 vector)
        {
            if (!_model.IsMove.Value)
            {
                _model.IsMove.SetValueAndForceNotify(true);
            }
            
            _model.CharacterController.transform.forward = vector;
        }
    }
}