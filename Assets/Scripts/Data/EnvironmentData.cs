using Enemy;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "EnvironmentData", menuName = "Data/Environment", order = 0)]
    public sealed class EnvironmentData : ScriptableObject
    {
        [SerializeField] private GameObject _trapDeathFX;
        [SerializeField] private GameObject _characterDeathFX;
        [SerializeField] private EnemyBehaviour _enemyBehaviour;

        public GameObject TrapDeathFX => _trapDeathFX;
        public GameObject CharacterDeathFX => _characterDeathFX;
        public EnemyBehaviour EnemyBehaviour => _enemyBehaviour;
    }
}