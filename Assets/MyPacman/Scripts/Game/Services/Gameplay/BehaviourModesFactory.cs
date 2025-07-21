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
        private readonly ReadOnlyReactiveProperty<Vector2> _pacmanPosition;

        public BehaviourModesFactory(
            MapHandlerService mapHandlerService,
            ReadOnlyReactiveProperty<Vector2> pacman,
            Vector2 mapSize,
            ReadOnlyReactiveProperty<Vector2> homePosition)
        {
            _mapHandlerService = mapHandlerService;
            _homePosition = homePosition;
            _pacmanPosition = pacman;
            InitScatterPositions(mapSize);
        }

        public GhostBehaviorMode CreateMode(GhostBehaviorModeType behaviorModeType, Ghost self)
        {
            switch (behaviorModeType)
            {
                case GhostBehaviorModeType.Chase:
                    return new BehaviourModeChase(_mapHandlerService, self, _pacmanPosition); // Передавать не self а ReadOnlyReactiveProperty конкретных параметров

                case GhostBehaviorModeType.Scatter:
                    return new BehaviourModeScatter(_mapHandlerService, self, _scatterPositions[self.Type]);

                case GhostBehaviorModeType.Frightened:
                    return new BehaviourModeFrightened(_mapHandlerService, self, _pacmanPosition);

                case GhostBehaviorModeType.Homecomming:
                    return new BehaviourModeHomecomming(_mapHandlerService, self, _homePosition.CurrentValue);

                default:
                    throw new System.Exception($"Unknown ghost behavior mode type: {behaviorModeType}");
            }
        }

        private void InitScatterPositions(Vector2 mapSize)
        {
            _scatterPositions.Add(EntityType.Blinky, new Vector2(0f, 0f));
            _scatterPositions.Add(EntityType.Pinky, new Vector2(mapSize.x, 0f));
            _scatterPositions.Add(EntityType.Inky, new Vector2(0f, mapSize.y));
            _scatterPositions.Add(EntityType.Clyde, new Vector2(mapSize.x, mapSize.y));
        }
    }
}