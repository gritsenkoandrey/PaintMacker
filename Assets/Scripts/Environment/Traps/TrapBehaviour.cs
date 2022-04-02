using System.Collections.Generic;
using BaseMonoBehaviour;
using UnityEngine;

namespace Environment.Traps
{
    public sealed class TrapBehaviour : BaseComponent
    {
        [SerializeField] private TrapModel _model;

        public TrapModel Model => _model;

        private readonly List<ITrap> _traps = new List<ITrap>();

        private void Construct()
        {
            _traps.Add(new TrapMove(_model));
            _traps.Add(new TrapCollision(_model));
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            Construct();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _traps.ForEach(trap => trap.Register());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
                        
            _traps.ForEach(trap => trap.Unregister());
        }
    }
}