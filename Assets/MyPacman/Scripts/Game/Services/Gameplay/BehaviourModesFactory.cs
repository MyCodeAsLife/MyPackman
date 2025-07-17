namespace MyPacman
{
    public class BehaviourModesFactory
    {
        private readonly MapHandlerService _mapHandlerService;

        public BehaviourModesFactory(MapHandlerService mapHandlerService)
        {
            _mapHandlerService = mapHandlerService;
        }

        public GhostBehaviorMode CreateMode(GhostBehaviorModeType behaviorModeType)
        {
            switch (behaviorModeType)
            {
                case GhostBehaviorModeType.Chase:
                    return new BehaviourModeChase(_mapHandlerService);

                case GhostBehaviorModeType.Scatter:
                    return new BehaviourModeScatter(_mapHandlerService);

                case GhostBehaviorModeType.Frightened:
                    return new BehaviourModeFrightened(_mapHandlerService);

                case GhostBehaviorModeType.Homecomming:
                    return new BehaviourModeHomecomming(_mapHandlerService);

                default:
                    throw new System.Exception($"Unknown ghost behavior mode type: {behaviorModeType}");
            }
        }
    }
}