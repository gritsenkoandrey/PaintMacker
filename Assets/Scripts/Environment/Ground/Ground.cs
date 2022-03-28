using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BaseMonoBehaviour;
using Levels;
using Managers;
using UniRx;
using UnityEngine;
using Utils;

namespace Environment.Ground
{
    public sealed class Ground : BaseComponent
    {
        [SerializeField] private Renderer _renderer;
        
        private Level _level;
        private Dictionary<GroundType, Action> _actions;

        public Pixel Pixel;
        public readonly ReactiveCommand<GroundType> OnChangeGround = new ReactiveCommand<GroundType>();

        protected override void Init()
        {
            base.Init();

            _level = Manager.Resolve<MWorld>().CurrentLevel.Value;

            OnChangeGround
                .Subscribe(type =>
                {
                    _actions[type].Invoke();
                })
                .AddTo(lifetimeDisposable);
            
            
            if (Pixel.index == 1)
            {
                OnChangeGround.Execute(GroundType.Frame);
            }
        }

        protected override void Enable()
        {
            base.Enable();

            _actions = new Dictionary<GroundType, Action>
            {
                { GroundType.Ground, SetGround },
                { GroundType.Frame, SetFrame },
                { GroundType.Forward, SetForward },
                { GroundType.Deactivate, SetDeactivate },
                { GroundType.Walking, SetWalking }
            };
        }

        protected override void Disable()
        {
            base.Disable();
            
            _actions.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetGround()
        {
            Pixel.index = 0;
            _renderer.material = _level.Materials.unpainted;
            gameObject.layer = Layers.Ground;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetFrame()
        {
            Pixel.index = 1;
            _renderer.material = _level.Materials.frame;
            gameObject.layer = Layers.Frame;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetForward()
        {
            Pixel.index = 2;
            _renderer.material = _level.Materials.frame;
            gameObject.layer = Layers.Path;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetDeactivate()
        {
            Pixel.index = 4;
            _renderer.material = _level.Materials.painted;
            gameObject.layer = Layers.Deactivate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetWalking()
        {
            _renderer.material = _level.Materials.frame;
            gameObject.layer = Layers.Walking;
        }
    }
}