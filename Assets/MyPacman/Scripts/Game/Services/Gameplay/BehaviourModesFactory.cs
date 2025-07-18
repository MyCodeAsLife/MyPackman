using System.Collections.Generic;
using UnityEngine;

namespace MyPacman
{
    public class BehaviourModesFactory
    {
        private readonly MapHandlerService _mapHandlerService;
        private readonly Dictionary<EntityType, Vector2> _scatterPositions = new();
        private readonly Vector2 _homePosition;
        private readonly Entity _pacman;

        public BehaviourModesFactory(
            MapHandlerService mapHandlerService,
            Entity pacman,
            Vector2 mapSize,
            Vector2 homePosition)
        {
            _mapHandlerService = mapHandlerService;
            _homePosition = homePosition;
            _pacman = pacman;
            InitScatterPositions(mapSize);
        }

        public GhostBehaviorMode CreateMode(GhostBehaviorModeType behaviorModeType, Ghost self)
        {
            switch (behaviorModeType)
            {
                case GhostBehaviorModeType.Chase:
                    return new BehaviourModeChase(_mapHandlerService, self, _pacman);

                case GhostBehaviorModeType.Scatter:
                    return new BehaviourModeScatter(_mapHandlerService, self, _scatterPositions[self.Type]);

                case GhostBehaviorModeType.Frightened:
                    return new BehaviourModeFrightened(_mapHandlerService, self, _pacman);

                case GhostBehaviorModeType.Homecomming:
                    return new BehaviourModeHomecomming(_mapHandlerService, self, _homePosition);

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