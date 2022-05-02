using System;
using AssetPath;
using Character;
using Cysharp.Threading.Tasks;
using Data;
using Environment.Ground;
using Levels;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Utils;

namespace Managers
{
    public sealed class MWorld : BaseManager
    {
        private LevelData _levelData;
        private int _index;

        [HideInPrefabs] public readonly ReactiveProperty<Level> CurrentLevel = new ReactiveProperty<Level>();
        public CharacterBehaviour Character { get; set; }
        public Ground[] Grounds { get; private set; } = Array.Empty<Ground>();
        public Ground[,] Grid { get; private set; } = new Ground[0,0];
        
        [HideInPrefabs] public readonly ReactiveProperty<int> Progress = new ReactiveProperty<int>();
        [HideInPrefabs] public readonly ReactiveCollection<Ground> PassedGround = new ReactiveCollection<Ground>();
        
        protected override void Enable()
        {
            base.Enable();
            
            _index = PlayerPrefs.GetInt(U.Level, 0);
            _levelData = CustomResources.Load<LevelData>(DataPath.Paths[DataType.Level]);
        }

        protected override void Launch()
        {
            base.Launch();
        }

        protected override void Disable()
        {
            base.Disable();

            Character = null;
            Grounds = Array.Empty<Ground>();
            Grid = new Ground[0,0];
            Progress.SetValueAndForceNotify(0);
            PassedGround.Clear();
        }

        public async UniTask LoadLevel(bool isWin)
        {
            Disable();

            if (CurrentLevel.Value)
            {
                Destroy(CurrentLevel.Value.gameObject);

                if (isWin)
                {
                    _index++;

                    if (_index == _levelData.GetLevels.Length)
                    {
                        _index = 0;
                    }

                    PlayerPrefs.SetInt(U.Level, _index);
                    PlayerPrefs.Save();
                }
            }

            CurrentLevel.SetValueAndForceNotify(SpawnLevel());

            await FillLevel();
        }

        private Level SpawnLevel() => 
            Instantiate(_levelData.GetLevels[_index], Vector3.zero, Quaternion.identity);

        private async UniTask FillLevel()
        {
            Grounds = CurrentLevel.Value.GetComponentsInChildren<Ground>();

            Grid = new Ground[30,50];

            foreach (Ground ground in Grounds)
            {
                Grid[ground.Pixel.x, ground.Pixel.y] = ground;
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }
    }
}