using System.Collections.Generic;
using BaseMonoBehaviour;
using UnityEngine;

namespace Character
{
    public sealed class CharacterBehaviour : BaseComponent
    {
        [SerializeField] private CharacterModel _model;

        public CharacterModel Model => _model;

        private readonly List<ICharacter> _characters = new List<ICharacter>();

        private void Construct()
        {
            _characters.Add(new CharacterCollision(_model));
            _characters.Add(new CharacterMove(_model));
            _characters.Add(new CharacterPaint(_model));
            _characters.Add(new CharacterAnimation(_model));
            _characters.Add(new CharacterCondition(_model));
        }

        protected override void Awake()
        {
            base.Awake();
            
            Construct();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _characters.ForEach(c => c.Register());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _characters.ForEach(c => c.Unregister());
        }
    }
}