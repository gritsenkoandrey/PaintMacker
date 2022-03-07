using System;
using System.Collections.Generic;
using UniRx;
using Utils;

namespace Character
{
    public sealed class CharacterAnimation : ICharacter
    {
        private readonly CharacterModel _model;
        private readonly Dictionary<AnimationType, Action> _animations;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public CharacterAnimation(CharacterModel model)
        {
            _model = model;

            _animations = new Dictionary<AnimationType, Action>
            {
                { AnimationType.Idle, Idle },
                { AnimationType.Run, Run },
                { AnimationType.Victory, Victory },
                { AnimationType.Death, Death }
            };
        }

        public void Register()
        {
            _model.OnVictory
                .Subscribe(value =>
                {
                    if (value)
                    {
                        _animations[AnimationType.Victory].Invoke();
                    }
                    else
                    {
                        _animations[AnimationType.Death].Invoke();

                    }
                })
                .AddTo(_disposable);

            _model.IsMove
                .Skip(1)
                .Subscribe(value =>
                {
                    if (value)
                    {
                        _animations[AnimationType.Run].Invoke();
                    }
                    else
                    {
                        _animations[AnimationType.Idle].Invoke();

                    }
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        private void Idle() => _model.Animator.SetTrigger(Animations.Idle);
        private void Run() => _model.Animator.SetTrigger(Animations.Run);
        private void Victory() => _model.Animator.SetTrigger(Animations.Victory);
        private void Death() => _model.Animator.SetTrigger(Animations.Death);
    }
}