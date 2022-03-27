using System;
using Cysharp.Threading.Tasks;
using Managers;
using UniRx;
using UnityEngine;
using Utils;

namespace Environment.Traps
{
    public sealed class TrapCollision : ITrap
    {
        private readonly TrapModel _model;
        
        private readonly MConfig _config;
        private readonly MPool _pool;
        private readonly MWorld _world;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public TrapCollision(TrapModel model)
        {
            _model = model;

            _config = Manager.Resolve<MConfig>();
            _pool = Manager.Resolve<MPool>();
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
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        private void CheckDeactivateGround()
        {
            if (Physics.Raycast(_model.Ray, 1f, Layers.GetDeactivate))
            {
                GameObject fx = _pool
                    .SpawnObject(_config.EnvironmentData.TrapDeathFX, _model.Transform.position, 
                        Quaternion.identity);

                ReturnToPool(fx);

                _model.GameObject.SetActive(false);
            }
        }

        private async void ReturnToPool(GameObject prefab)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            
            _pool.ReleaseObject(prefab);
        }
    }
}