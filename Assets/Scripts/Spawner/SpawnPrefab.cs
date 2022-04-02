using System;
using System.Collections.Generic;
using BaseMonoBehaviour;
using Character;
using Managers;
using UnityEngine;
using Utils;

namespace Spawner
{
    public sealed class SpawnPrefab : BaseComponent
    {
        [SerializeField] private SpawnPrefabType _prefabType;
        [SerializeField] private Color _color = Color.green;
        [SerializeField, Range(0.1f, 1f)] private float _radius = 0.25f;
        
        private MConfig _config;
        private MWorld _world;

        private Dictionary<SpawnPrefabType, Action> _spawn;

        protected override void Awake()
        {
            base.Awake();
            
            _config = Manager.Resolve<MConfig>();
            _world = Manager.Resolve<MWorld>();

            _spawn = new Dictionary<SpawnPrefabType, Action>
            {
                { SpawnPrefabType.None, () => CustomDebug.Log("Prefab Type Not Selected")},
                { SpawnPrefabType.Character, SpawnCharacter},
                { SpawnPrefabType.Enemy, SpawnEnemy }
            };
        }

        protected override void Start()
        {
            base.Start();
            
            _spawn[_prefabType].Invoke();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _spawn.Clear();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            
            Gizmos.DrawSphere(transform.position, _radius);
        }

        private T InstantiatePrefab<T>(T prefab) where T : BaseComponent
        {
            return Instantiate(prefab, transform.position,
                Quaternion.identity, _world.CurrentLevel.Value.transform);
        }

        private void SpawnCharacter()
        {
            CharacterBehaviour character = InstantiatePrefab(_config.CharacterData.Character);

            _world.Character = character;
            _world.CurrentLevel.Value.Cameras[1].Follow = character.transform;
            _world.CurrentLevel.Value.Cameras[1].LookAt = character.transform;
        }

        private void SpawnEnemy()
        {
            InstantiatePrefab(_config.EnvironmentData.EnemyBehaviour);
        }
    }
}