using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Environment.Ground;
using Managers;
using UniRx;
using UnityEngine;
using Utils;

namespace Character
{
    public sealed class CharacterCondition : ICharacter
    {
        private readonly CharacterModel _model;
        
        private readonly MGame _game;
        private readonly MWorld _world;
        private readonly MConfig _config;
        private readonly MPool _pool;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public CharacterCondition(CharacterModel model)
        {
            _model = model;
            
            _game = Manager.Resolve<MGame>();
            _world = Manager.Resolve<MWorld>();
            _config = Manager.Resolve<MConfig>();
            _pool = Manager.Resolve<MPool>();
        }

        public void Register()
        {
            _model.OnVictory
                .First()
                .Subscribe(isVictory =>
                {
                    _world.CurrentLevel.Value.Cameras[1].Priority = 100;
                    
                    if (isVictory)
                    {
                        _world.Progress.SetValueAndForceNotify(100);
                            
                        foreach (Ground ground in _world.Grounds.Where(g => g.Pixel.index != 1))
                        {
                            ground.OnChangeGround.Execute(GroundType.Deactivate);
                        }
                        
                        _world.PassedGround.Clear();
                    }
                    else
                    {
                        GameObject fx = _pool
                            .SpawnObject(_config.EnvironmentData.CharacterDeathFX, 
                                _model.Transform.position, Quaternion.identity);
                        
                        ReturnToPool(fx);

                        int index = 0;
                        
                        foreach (Ground ground in _world.PassedGround)
                        {
                            DestroyGround(ground, (float)index / _world.PassedGround.Count);
                            index++;
                        }
                    }
                    
                    _model.CharacterDisposable.Clear();
                    
                    _game.OnRoundEnd.Execute(isVictory);
                })
                .AddTo(_disposable);

            _world.PassedGround
                .ObserveReset()
                .Subscribe(_ =>
                {
                    int count = _world.Grounds
                        .Count(g => g.Pixel.index == 4);
                        
                    _world.Progress
                        .SetValueAndForceNotify((int)U.Remap(0, U.MaxGround, 0, 100, count));

                    if (count > _config.SettingsData.GetCountToWin)
                    {
                        _model.OnVictory.Execute(true);
                    }
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        private async void ReturnToPool(GameObject prefab)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            
            _pool.ReleaseObject(prefab);
        }

        private static async void DestroyGround(Ground ground, float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            ground.OnChangeGround.Execute(GroundType.Ground);
        }
    }
}