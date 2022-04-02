using Enemy;
using Environment.Ground;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "EnvironmentData", menuName = "Data/Environment", order = 0)]
    public sealed class EnvironmentData : ScriptableObject
    {
        [SerializeField] private GameObject _trapDeathFX;
        [SerializeField] private GameObject _characterDeathFX;
        [SerializeField] private GameObject _enemyTarget;
        [SerializeField] private EnemyBehaviour _enemyBehaviour;
        [SerializeField] private Materials _materials;

        public GameObject TrapDeathFX => _trapDeathFX;
        public GameObject CharacterDeathFX => _characterDeathFX;
        public GameObject EnemyTarget => _enemyTarget;
        public EnemyBehaviour EnemyBehaviour => _enemyBehaviour;
        public Materials Materials => _materials;
    }
}