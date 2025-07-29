using R3;
using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    public class BehaviourModesFactory
    {
        private readonly MapHandlerService _mapHandlerService;
        private readonly Dictionary<EntityType, Vector2> _scatterPositions = new();
        private readonly ReadOnlyReactiveProperty<Vector2> _homePosition;
        private readonly ReadOnlyReactiveProperty<Vector2> _blinkyPosition;
        private readonly ReadOnlyReactiveProperty<Vector2> _pacmanPosition;
        private readonly ReadOnlyReactiveProperty<Vector2> _pacmanDirection;

        public BehaviourModesFactory(
            MapHandlerService mapHandlerService,
            ReadOnlyReactiveProperty<Vector2> blinkyPosition,
            ReadOnlyReactiveProperty<Vector2> pacmanPosition,
            ReadOnlyReactiveProperty<Vector2> pacmanDirection,
            Vector2 mapSize,
            ReadOnlyReactiveProperty<Vector2> homePosition)
        {
            _mapHandlerService = mapHandlerService;
            _homePosition = homePosition;
            _blinkyPosition = blinkyPosition;
            _pacmanPosition = pacmanPosition;
            _pacmanDirection = pacmanDirection;
            InitScatterPositions(mapSize);
        }

        // Передавать не self а ReadOnlyReactiveProperty конкретных параметров
        public GhostBehaviorMode CreateMode(GhostBehaviorModeType behaviorModeType, Ghost self)
        {
            switch (behaviorModeType)
            {
                case GhostBehaviorModeType.Chase:
                    return GetBehaviourModeChase(self);

                case GhostBehaviorModeType.Scatter:
                    return new BehaviourModeScatter(
                        _mapHandlerService,
                        self,
                        _scatterPositions[self.Type],
                        behaviorModeType);

                case GhostBehaviorModeType.Frightened:
                    return new BehaviourModeFrightened(_mapHandlerService, self, _pacmanPosition);

                case GhostBehaviorModeType.Homecomming:
                    return new BehaviourModeScatter(
                        _mapHandlerService,
                        self,
                        _homePosition.CurrentValue,
                        behaviorModeType);

                default:
                    throw new System.Exception($"Unknown ghost behavior mode type: {behaviorModeType}");    // Magic
            }
        }

        private void InitScatterPositions(Vector2 mapSize)
        {
            _scatterPositions.Add(EntityType.Blinky, new Vector2(0f, 0f));
            _scatterPositions.Add(EntityType.Pinky, new Vector2(mapSize.x, 0f));
            _scatterPositions.Add(EntityType.Inky, new Vector2(0f, mapSize.y));
            _scatterPositions.Add(EntityType.Clyde, new Vector2(mapSize.x, mapSize.y));
        }

        private BehaviourModeChase GetBehaviourModeChase(Ghost self)
        {
            switch (self.Type)
            {
                case EntityType.Blinky:
                    return new BlinkyBehaviourModeChase(_mapHandlerService, self, _pacmanPosition);

                case EntityType.Pinky:
                    return new PinkyBehaviourModeChase(_mapHandlerService, self, _pacmanPosition, _pacmanDirection);

                case EntityType.Inky:
                    return new InkyBehaviourModeChase(_mapHandlerService, self, _pacmanPosition, _blinkyPosition);

                case EntityType.Clyde:
                    return new ClydeBehaviourModeChase(
                        _mapHandlerService,
                        self,
                        _pacmanPosition,
                        _scatterPositions[self.Type]);

                default:
                    throw new System.Exception($"Unknown ghost type: {self.Type}");                     //Magic
            }
        }
    }
}