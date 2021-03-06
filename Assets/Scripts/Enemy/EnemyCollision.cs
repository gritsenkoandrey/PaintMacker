using System;
using Cysharp.Threading.Tasks;
using Managers;
using UniRx;
using UnityEngine;
using Utils;

namespace Enemy
{
    public sealed class EnemyCollision : IEnemy
    {
        private readonly EnemyModel _model;
        
        private readonly MPool _pool;
        private readonly MConfig _config;
        private readonly MWorld _world;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public EnemyCollision(EnemyModel model)
        {
            _model = model;

            _pool = Manager.Resolve<MPool>();
            _config = Manager.Resolve<MConfig>();
            _world = Manager.Resolve<MWorld>();
        }

        public void Register()
        {
            _world.PassedGround
                .ObserveReset()
                .Subscribe(_ => CheckDeactivateGround())
                .AddTo(_disposable);

            _world.PassedGround
                .ObserveAdd()
                .Subscribe(_ => CheckCharacterPath())
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        private void CheckDeactivateGround()
        {
            if (Physics.Raycast(_model.Ray, 1f, Layers.GetDeactivate))
            {
                DeathEnemy();
            }
            else
            {
                if (!_model.Agent.CalculatePath(_model.EndPath.position, _model.Agent.path))
                {
                    _model.OnGeneratePath.Execute();
                }
            }
        }

        private void CheckCharacterPath()
        {
            if (Physics.Raycast(_model.Ray, 1f, Layers.GetPath))
            {
                _world.Character.Model.OnVictory.Execute(false);
            }
        }

        private void DeathEnemy()
        {
            _model.Transform.gameObject.SetActive(false);
            
            GameObject fx = _pool.SpawnObject(_config.EnvironmentData.TrapDeathFX, 
                _model.Transform.position, Quaternion.identity);
                        
            ReturnToPool(fx);
        }

        private async void ReturnToPool(GameObject fx)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            
            _pool.ReleaseObject(fx);
        }
    }
}