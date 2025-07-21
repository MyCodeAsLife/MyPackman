using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    // Преследование
    public class BehaviourModeChase : GhostBehaviorMode
    {
        // Смещение для расчета целевой точки, для каждого призрака свое.
        private Vector2 _targetPointOffset;
        private ReadOnlyReactiveProperty<Vector2> _pacmanPosition;      // Это нужно?

        public BehaviourModeChase(MapHandlerService mapHandlerService, Ghost self, ReadOnlyReactiveProperty<Vector2> pacmanPosition)
            : base(mapHandlerService, self, GhostBehaviorModeType.Chase)
        {
            _pacmanPosition = pacmanPosition;
            pacmanPosition.Subscribe(newPos => _targetPosition.OnNext(newPos)); // Будет ли ошибка если этот класс удалится а данная лямбда останется подписанна?
        }

        protected override Vector2 CalculateDirectionInSelectedMode(List<Vector2> availableDirections)
        {
            throw new NotImplementedException();
        }
    }
}
