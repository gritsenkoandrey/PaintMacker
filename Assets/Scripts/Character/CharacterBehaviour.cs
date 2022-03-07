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

        public void Construct()
        {
            _characters.Add(new CharacterCollision(_model));
            _characters.Add(new CharacterMove(_model));
            _characters.Add(new CharacterPaint(_model));
            _characters.Add(new CharacterAnimation(_model));
            _characters.Add(new CharacterCondition(_model));
        }
        
        protected override void Init()
        {
            base.Init();
            
            _characters.ForEach(c => c.Register());
        }

        protected override void Enable()
        {
            base.Enable();
        }

        protected override void Disable()
        {
            base.Disable();
            
            _characters.ForEach(c => c.Unregister());
        }
    }
}