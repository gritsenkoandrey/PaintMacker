using Character;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Data/Character", order = 0)]
    public sealed class CharacterData : ScriptableObject
    {
        [SerializeField] private CharacterBehaviour _character;
        [SerializeField] private float _speed;

        public CharacterBehaviour Character => _character;
        public float Speed => _speed;
    }
}