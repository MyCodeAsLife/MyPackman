namespace MyPacman
{
    public static class GameConstants   // Разделить на GameConstants и AppConstants?
    {
        // General
        public const float Half = 0.5f;

        // Scene names
        public const string BootScene = nameof(BootScene);
        public const string MainMenuScene = nameof(MainMenuScene);
        public const string GameplayScene = nameof(GameplayScene);

        // Layers and Tilemaps
        public const string Obstacle = nameof(Obstacle);
        //public const string Pellet = nameof(Pellet);
        //public const string Fruit = nameof(Fruit);

        // LayerMask
        public const int LayerMaskEverything = -1;

        // Paths to prefabs
        public const string PacmanFolderPath = "Prefabs/";
        //public const string PacmanTestFullPath = "Prefabs/PacmanTest";      // Выпилить
        //public const string PacmanNewFullPath = "Prefabs/NewPacman";
        public const string NoFrictionMaterialFullPath = "Prefabs/NoFrictionMaterial";    // Материал без трения
        public const string WallTilesFolderPath = "Assets/WallTiles/";
        //public const string FruitRuleTilesFolderPath = "Assets/FruitsRuleTiles/";
        //public const string PelletRuleTilesFolderPath = "Assets/PelletsRuleTiles/";
        public const string GhostsFolderPath = "Prefabs/Ghosts/";
        public const string PelletsFolderPath = "Prefabs/Pellets/";
        public const string FruitsFolderPath = "Prefabs/Fruits/";
        public const string IconsFolderPath = "Prefabs/Fruits/Icons/";
        public const string UIFolderPath = "Prefabs/UI/";
        public const string UIRootViewFullPath = "Prefabs/UI/UIRootView";
        //public const string UIMainMenuFullPath = "Prefabs/UI/MainMenuUI";
        public const string UIGameplayFullPath = "Prefabs/UI/UIGameplayRootView";

        // Number of tiles along the specified path
        public const int NumberOfWallTiles = 38;
        public const int NumberOfPelletTiles = 3;
        //public const int NumberOfFruitTiles = 1;         // Количество подбираемых вещей в папке Prefabs(apple, key и т.д.)

        // Map settings
        public const float GridCellSize = 1f;
        //public const int GridCellPixelSize = 24;
        //public const int FieldsAtTheEdgesOfTheMap = 1;  // Отступы по краям карты

        // Gameplay settings
        public const float PlayerSpeed = 6f;
        public const float PlayerInvincibleTimer = 2.5f;
        public const float ScatterTimer = 6f;
        public const float ChaseTimer = 6f;
        public const float GhostSpeed = 6f;
        public const float GhostNormalSpeed​​Modifier = 1f;
        public const float GhostTunelSpeed​​Modifier = 0.6f;
        public const int StartLifePointsAmount = 3;
        public const int StartingDifficultyLevel = 1;
        public const int CollectedPelletsForFirstFruitSpawn = 10;           // 70       Переделать под процент от всех пеллетов на уровне
        public const int CollectedPelletsForSecondFruitSpawn = 15;         // 170      Переделать под процент от всех пеллетов на уровне
        public const int PriceLifePoint = 10000;
        public const int MaxNumberFruitIconOnPanel = 10;
        public const int MaxNumberLifeIconOnPanel = 10;

        // Level Constructor - переделать в enum?
        //public const int EmptyTile = 0;
        public const int GateTile = 37;
        public const int SpeedModifierTile = -2;
        //public const int PacmanSpawn = (int)SpawnPointType.Pacman;
        //public const int BlinkySpawn = (int)SpawnPointType.Blinky;
        //public const int PinkySpawn = (int)SpawnPointType.Pinky;
        //public const int InkySpawn = (int)SpawnPointType.Inky;
        //public const int ClydeSpawn = (int)SpawnPointType.Clyde;
        //public const int FruitSpawn = (int)SpawnPointType.Fruit;
        //public const int SmallPellet = -3;
        //public const int MediumPellet = -4;
        //public const int LargePellet = -5;

        //// Errors
        //public const string PositionOnMapNotFound = "Position on map, not Found.";

        // UI and Camera
        public const float GameplayInformationalPamelHeight = 3f;

        // Signal Tag
        public const string SceneExitRequestTag = nameof(SceneExitRequestTag);
        //public const string RequestTagWhenDeathAnimationEnds = nameof(RequestTagWhenDeathAnimationEnds);

        // Errors
        public const string NoSwitchingDefined = "There is no switching defined for this behavior. Behavior: ";
    }
}