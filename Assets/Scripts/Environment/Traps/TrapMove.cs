using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace Environment.Traps
{
    public sealed class TrapMove : ITrap
    {
        private readonly TrapModel _model;
        
        private readonly Dictionary<TrapType, Action<TrapSettings>> _initTrap;

        public TrapMove(TrapModel model)
        {
            _model = model;
            
            _initTrap = new Dictionary<TrapType, Action<TrapSettings>>
            {
                { TrapType.None, trapSettings => CustomDebug.Log("Trap Type Is Not Selected") },
                { TrapType.Barrier, InitBarrier },
                { TrapType.Canon, InitCanon },
                { TrapType.Cylinder, InitCylinder },
                { TrapType.SawVertical, InitSawVertical },
                { TrapType.Spikes, InitSpikes },
                { TrapType.SawHorizontal, InitSawHorizontal }
            };
        }
        
        public void Register()
        {
            _model.Sequence = _model.Sequence.RefreshSequence();
            
            _initTrap[_model.TrapSettings.type].Invoke(_model.TrapSettings);
        }

        public void Unregister()
        {
            _model.Sequence.KillTween();
        }
        
        private void InitBarrier(TrapSettings s)
        {
            _model.Sequence
                .Append(_model.Transform
                    .DOLocalRotate(Vector3.up * 360f, s.duration, RotateMode.WorldAxisAdd)
                    .SetEase(Ease.Linear))
                .SetLoops(-1);
        }

        private void InitCylinder(TrapSettings s)
        {
            _model.Sequence
                .Append(_model.Transform
                    .DOLocalRotate(Vector3.up * 360f, s.duration, RotateMode.WorldAxisAdd)
                    .SetEase(Ease.Linear))
                .SetLoops(-1);
        }

        private void InitSawVertical(TrapSettings s)
        {
            const float rotateTime = 1f;

            _model.Sequence
                .Append(_model.Transform
                    .DOLocalMoveZ(s.vector.z, s.duration)
                    .SetRelative()
                    .SetEase(Ease.Linear))
                .Join(_model.Transform
                    .DOLocalRotate(Vector3.right * s.vector.x * 360f, rotateTime, RotateMode.WorldAxisAdd)
                    .SetEase(Ease.Linear)
                    .SetLoops(Mathf.FloorToInt(s.duration / rotateTime)))
                .AppendInterval(s.delay)
                .Append(_model.Transform
                    .DOLocalMoveZ(-s.vector.z, s.duration)
                    .SetRelative()
                    .SetEase(Ease.Linear))
                .Join(_model.Transform
                    .DOLocalRotate(Vector3.right * -s.vector.x * 360f, rotateTime, RotateMode.WorldAxisAdd)
                    .SetEase(Ease.Linear)
                    .SetLoops(Mathf.FloorToInt(s.duration / rotateTime)))
                .AppendInterval(s.delay)
                .SetLoops(-1);
        }

        private void InitSpikes(TrapSettings s)
        {
            _model.Sequence
                .Append(_model.Transform.DOMove(s.vector, s.duration))
                .SetRelative()
                .SetEase(Ease.InBack)
                .AppendInterval(s.delay)
                .Append(_model.Transform.DOMove(-s.vector, s.duration))
                .SetRelative()
                .SetEase(Ease.OutBack)
                .SetLoops(-1);
        }

        private void InitCanon(TrapSettings s)
        {
            _model.Sequence
                .Append(_model.Transform.DOLocalRotate(s.vector, s.duration))
                .SetRelative()
                .SetEase(Ease.Linear)
                .AppendInterval(s.delay)
                .Append(_model.Transform.DOLocalRotate(-s.vector, s.duration))
                .SetRelative()
                .SetEase(Ease.Linear)
                .AppendInterval(s.delay)
                .SetLoops(-1);
        }

        private void InitSawHorizontal(TrapSettings s)
        {
            _model.Sequence
                .Append(_model.Transform
                    .DOLocalRotate(Vector3.up * 360f, s.duration, RotateMode.WorldAxisAdd)
                    .SetEase(Ease.Linear))
                .SetLoops(-1);
        }
    }
}