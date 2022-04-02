using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BaseMonoBehaviour;
using Managers;
using UniRx;
using UnityEngine;
using Utils;

namespace Environment.Ground
{
    public sealed class Ground : BaseComponent
    {
        [SerializeField] private Renderer _renderer;

        private MConfig _config;
        
        private Dictionary<GroundType, Action> _actions;

        public Pixel Pixel;

        public readonly ReactiveCommand<GroundType> OnChangeGround = new ReactiveCommand<GroundType>();

        protected override void Awake()
        {
            base.Awake();

            _config = Manager.Resolve<MConfig>();

            _actions = new Dictionary<GroundType, Action>
            {
                { GroundType.Ground, SetGround },
                { GroundType.Frame, SetFrame },
                { GroundType.Forward, SetForward },
                { GroundType.Deactivate, SetDeactivate },
                { GroundType.Walking, SetWalking }
            };
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            OnChangeGround
                .Subscribe(type => _actions[type].Invoke())
                .AddTo(this);
        }

        protected override void Start()
        {
            base.Start();
            
            if (Pixel.index == 1)
            {
                SetFrame();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _actions.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetGround()
        {
            Pixel.index = 0;
            _renderer.material = _config.EnvironmentData.Materials.unpainted;
            gameObject.layer = Layers.Ground;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetFrame()
        {
            Pixel.index = 1;
            _renderer.material = _config.EnvironmentData.Materials.frame;
            gameObject.layer = Layers.Frame;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetForward()
        {
            Pixel.index = 2;
            _renderer.material = _config.EnvironmentData.Materials.frame;
            gameObject.layer = Layers.Path;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetDeactivate()
        {
            Pixel.index = 4;
            _renderer.material = _config.EnvironmentData.Materials.painted;
            gameObject.layer = Layers.Deactivate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetWalking()
        {
            _renderer.material = _config.EnvironmentData.Materials.frame;
            gameObject.layer = Layers.Walking;
        }
    }
}