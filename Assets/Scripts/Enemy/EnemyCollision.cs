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
                .Subscribe(_ =>
                {
                    CheckDeactivateGround();
                })
                .AddTo(_disposable);

            _world.PassedGround
                .ObserveAdd()
                .Subscribe(_ =>
                {
                    CheckCharacterPath();
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        private void CheckDeactivateGround()
        {
            Ray ray = new Ray { origin = _model.Transform.position, direction = Vector3.down };
            
            if (Physics.Raycast(ray, 1f, Layers.GetDeactivate))
            {
                _model.Transform.gameObject.SetActive(false);

                GameObject fx = _pool
                    .SpawnObject(_config.EnvironmentData.TrapDeathFX, _model.Transform.position, 
                        Quaternion.identity);
                        
                ReturnToPool(fx);
            }
            else
            {
                _model.OnGeneratePath.Execute();
            }
        }

        private void CheckCharacterPath()
        {
            Ray ray = new Ray { origin = _model.Transform.position, direction = Vector3.down };

            if (Physics.Raycast(ray, 1f, Layers.GetPath))
            {
                _world.Character.Model.OnVictory.Execute(false);
            }
        }

        private async void ReturnToPool(GameObject fx)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            
            _pool.ReleaseObject(fx);
        }
    }
}