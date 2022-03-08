using System.Linq;
using Environment.Ground;
using Managers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

namespace Character
{
    public sealed class CharacterCollision : ICharacter
    {
        private readonly CharacterModel _model;
        
        private readonly MWorld _world;

        private Collider _collider;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public CharacterCollision(CharacterModel model)
        {
            _model = model;
            
            _world = Manager.Resolve<MWorld>();
        }
        
        public void Register()
        {
            _collider = _model.CharacterController;
            
            Observable
                .EveryUpdate()
                .Where(_ => _model.IsMove.Value)
                .Subscribe(_ =>
                {
                    if (!GeneratePath()) return;

                    StepOnGeneratePath();
                })
                .AddTo(_model.CharacterDisposable)
                .AddTo(_disposable);

            _model.CharacterController
                .OnTriggerEnterAsObservable()
                .Where(col => col.gameObject.layer == Layers.Trap)
                .First()
                .Subscribe(c =>
                {
                    _model.OnVictory.Execute(false);
                })
                .AddTo(_model.CharacterDisposable)
                .AddTo(_disposable);
            
            _model.CharacterController
                .OnTriggerEnterAsObservable()
                .Where(col => col.gameObject.layer == Layers.Agent)
                .First()
                .Subscribe(c =>
                {
                    _model.OnVictory.Execute(false);
                })
                .AddTo(_model.CharacterDisposable)
                .AddTo(_disposable);

            _world.PassedGround
                .ObserveReset()
                .Subscribe(_ =>
                {
                    _collider = _model.CharacterController;
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        private bool GeneratePath()
        {
            Ray rayCenter = new Ray { origin = _model.Center.position, direction = Vector3.down };

            if (Physics.Raycast(rayCenter, out RaycastHit hit, 1f, Layers.GetGround))
            {
                if (_collider.Equals(hit.collider)) return false;

                _collider = hit.collider;

                Ground ground = _world.Grounds
                    .First(g => g.gameObject.Equals(_collider.gameObject));

                _world.PassedGround.Add(ground);

                ground.OnChangeGround.Execute(GroundType.Forward);
            }

            return true;
        }

        private void StepOnGeneratePath()
        {
            Ray rayPath = new Ray { origin = _model.Path.position, direction = Vector3.down };

            if (Physics.Raycast(rayPath, 1f, Layers.GetPath))
            {
                _model.OnVictory.Execute(false);
            }
        }
    }
}