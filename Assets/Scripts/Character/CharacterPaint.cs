using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Environment.Ground;
using Managers;
using UniRx;
using Utils;

namespace Character
{
    public sealed class CharacterPaint : ICharacter
    {
        private readonly MWorld _world;
        
        private readonly CharacterModel _model;

        private readonly List<Ground> _first = new List<Ground>();
        private readonly List<Ground> _second = new List<Ground>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public CharacterPaint(CharacterModel model)
        {
            _model = model;
            
            _world = Manager.Resolve<MWorld>();
        }

        public void Register()
        {
            _model.IsMove
                .Where(value => !value)
                .Subscribe(_ =>
                {
                    if (_world.PassedGround.Count > 0)
                    {
                        PaintGrid();
                        PaintBounds();
                    }
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PaintGrid()
        {
            Ground ground = _world.PassedGround.FirstOrDefault(IsNoPaintGround);

            if (ground)
            {
                CheckGrid(ground.Pixel.x, ground.Pixel.y);
                        
                if (_first.Count > 0 && _first.Count <= _second.Count)
                {
                    _first.ForEach(Deactivate);
                }
                else if (_second.Count > 0 && _second.Count <= _first.Count)
                {
                    _second.ForEach(Deactivate);
                }
            }

            _world.PassedGround.ForEach(Deactivate);
            
            _world.PassedGround.ForEach(PassedGround);
                    
            _world.PassedGround.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PaintBounds()
        {
            foreach (Ground g in _world.Grounds)
            {
                if (g.Pixel.index == 4)
                {
                    CheckBounds(g.Pixel.x, g.Pixel.y);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckGrid(int x, int y)
        {
            _first.Clear();
            _second.Clear();

            if (_world.Grid[x + 1, y].Pixel.index == 0 || 
                _world.Grid[x - 1, y].Pixel.index == 0)
            {
                if (_world.Grid[x + 1, y].Pixel.index == 0)
                {
                    CheckSide(x + 1, y, _first);
                }

                if (_world.Grid[x - 1, y].Pixel.index == 0)
                {
                    CheckSide(x - 1, y, _second);
                }
            }
            else if (_world.Grid[x, y + 1].Pixel.index == 0 || 
                     _world.Grid[x, y - 1].Pixel.index == 0)
            {
                if (_world.Grid[x, y + 1].Pixel.index == 0)
                {
                    CheckSide(x, y + 1, _first);
                }

                if (_world.Grid[x, y - 1].Pixel.index == 0)
                {
                    CheckSide(x, y - 1, _second);
                }
            }
            else
            {
                return;
            }
            
            _first.ForEach(ResetPaintGround);
            _second.ForEach(ResetPaintGround);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckSide(int x, int y, ICollection<Ground> g)
        {
            if (_world.Grid[x, y].Pixel.index != 0) return;
            
            g.Add(_world.Grid[x, y]);
                
            _world.Grid[x, y].Pixel.index = 5;

            CheckSide(x, y - 1, g);
            CheckSide(x, y + 1, g);
            CheckSide(x - 1, y, g);
            CheckSide(x + 1, y, g);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckBounds(int x, int y)
        {
            if (_world.Grid[x + 1, y + 1].Pixel.index == 0 || 
                _world.Grid[x - 1, y - 1].Pixel.index == 0 || 
                _world.Grid[x + 1, y - 1].Pixel.index == 0 || 
                _world.Grid[x - 1, y + 1].Pixel.index == 0)
            {
                _world.Grid[x, y].OnChangeGround.Execute(GroundType.Walking);
            }
            else
            {
                _world.Grid[x, y].OnChangeGround.Execute(GroundType.Deactivate);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckBoundsPassedGround(int x, int y)
        {
            if (_world.Grid[x + 1, y].Pixel.index == 0 && _world.Grid[x - 1, y].Pixel.index == 0 ||
                _world.Grid[x, y + 1].Pixel.index == 0 && _world.Grid[x, y - 1].Pixel.index == 0)
            {
                _world.Grid[x, y].OnChangeGround.Execute(GroundType.Ground);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Deactivate(Ground ground) => ground.OnChangeGround.Execute(GroundType.Deactivate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PassedGround(Ground ground) => CheckBoundsPassedGround(ground.Pixel.x, ground.Pixel.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsNoPaintGround(Ground ground)
        {
            if (_world.Grid[ground.Pixel.x + 1, ground.Pixel.y].Pixel.index == 0 &&
                _world.Grid[ground.Pixel.x - 1, ground.Pixel.y].Pixel.index == 0 ||
                _world.Grid[ground.Pixel.x, ground.Pixel.y + 1].Pixel.index == 0 &&
                _world.Grid[ground.Pixel.x, ground.Pixel.y - 1].Pixel.index == 0)
            {
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResetPaintGround(Ground ground) => ground.Pixel.index = 0;
    }
}