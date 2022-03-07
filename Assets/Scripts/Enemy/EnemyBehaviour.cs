using System.Collections.Generic;
using BaseMonoBehaviour;
using UnityEngine;

namespace Enemy
{
    public sealed class EnemyBehaviour : BaseComponent
    {
        [SerializeField] private EnemyModel _model;

        public EnemyModel Model => _model;
        
        private readonly List<IEnemy> _enemies = new List<IEnemy>();

        public void Construct()
        {
            _enemies.Add(new EnemyMove(_model));
            _enemies.Add(new EnemyAnimation(_model));
            _enemies.Add(new EnemyCollision(_model));
        }

        protected override void Init()
        {
            base.Init();
            
            _enemies.ForEach(e => e.Register());
        }

        protected override void Enable()
        {
            base.Enable();
        }

        protected override void Disable()
        {
            base.Disable();
            
            _enemies.ForEach(e => e.Unregister());
        }
    }
}