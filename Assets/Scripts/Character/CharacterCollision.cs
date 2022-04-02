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
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public CharacterCollision(CharacterModel model)
        {
            _model = model;
            
            _world = Manager.Resolve<MWorld>();
        }
        
        public void Register()
        {
            _model.CharacterController
                .UpdateAsObservable()
                .Where(_ => _model.IsMove.Value)
                .Subscribe(_ => GeneratePath())
                .AddTo(_model.CharacterDisposable)
                .AddTo(_disposable);

            _model.CharacterController
                .OnTriggerEnterAsObservable()
                .Where(collider => 
                    collider.gameObject.layer == Layers.Trap || collider.gameObject.layer == Layers.Agent)
                .First()
                .Subscribe(_ => _model.OnVictory.Execute(false))
                .AddTo(_model.CharacterDisposable)
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        private void GeneratePath()
        {
            if (Physics.Raycast(_model.RayCenter, out RaycastHit hit, 1f, Layers.GetGround))
            {
                if (_world.PassedGround.Count > 0 &&
                    _world.PassedGround.GetLast().gameObject.Equals(hit.collider.gameObject))
                {
                    return;
                }
                
                Ground ground = _world.Grounds
                    .First(g => g.gameObject.Equals(hit.collider.gameObject));

                _world.PassedGround.Add(ground);

                ground.OnChangeGround.Execute(GroundType.Forward);
                
                return;
            }
            
            StepOnGeneratePath();
        }

        private void StepOnGeneratePath()
        {
            if (Physics.Raycast(_model.RayPath, 1f, Layers.GetPath))
            {
                _model.OnVictory.Execute(false);
            }
        }
    }
}