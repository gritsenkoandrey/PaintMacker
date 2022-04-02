using System;
using DG.Tweening;
using UI.Enum;
using UI.Factory;
using UniRx;

namespace Managers
{
    public sealed class MGame : BaseManager
    {
        private MConfig _config;
        private MGUI _gui;
        private MWorld _world;
        private MInput _input;
        
        public readonly ReactiveCommand OnRoundStart = new ReactiveCommand();
        public readonly ReactiveCommand<bool> OnRoundEnd = new ReactiveCommand<bool>();
        public readonly ReactiveCommand<bool> LaunchRound = new ReactiveCommand<bool>();

        private readonly CompositeDisposable _gameDisposable = new CompositeDisposable();

        protected override void Init()
        {
            base.Init();
            
            _config = Manager.Resolve<MConfig>();
            _gui = Manager.Resolve<MGUI>();
            _world = Manager.Resolve<MWorld>();
            _input = Manager.Resolve<MInput>();

            OnRoundStart
                .Subscribe(_ =>
                {
                    ScreenInterface
                        .GetScreenInterface()
                        .Execute(ScreenType.GameScreen);
                    
                    _input.IsEnable.SetValueAndForceNotify(true);
                })
                .AddTo(ManagerDisposable);
            
            OnRoundEnd
                .Subscribe(value =>
                {
                    float time = value ? _config.SettingsData.GetTimeToWin : _config.SettingsData.GetTimeToLose;

                    Observable
                        .Timer(TimeSpan.FromSeconds(time))
                        .Subscribe(_ =>
                        {
                            ScreenInterface
                                .GetScreenInterface()
                                .Execute(value ? ScreenType.WinScreen : ScreenType.LoseScreen);
                        })
                        .AddTo(_gameDisposable);
                    
                    _input.IsEnable.SetValueAndForceNotify(false);
                })
                .AddTo(ManagerDisposable);

            LaunchRound
                .Subscribe(async value =>
                {
                    Clear();
                    
                    ScreenInterface
                        .GetScreenInterface()
                        .Execute(ScreenType.None);

                    _gui.GetFade
                        .DOFade(1f, 0f);

                    await _world.LoadLevel(value);
                                        
                    ScreenInterface
                        .GetScreenInterface()
                        .Execute(ScreenType.LobbyScreen);

                    _gui.GetFade
                        .DOFade(0f, 0.1f)
                        .SetEase(Ease.Linear);
                })
                .AddTo(ManagerDisposable);
        }

        protected override void Launch()
        {
            base.Launch();
            
            LaunchRound.Execute(false);
        }

        protected override void Clear()
        {
            _gameDisposable.Clear();
        }
    }
}