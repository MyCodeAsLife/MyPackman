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
        public const string Pellet = nameof(Pellet);
        public const string Fruit = nameof(Fruit);

        // LayerMask
        public const int LayerMaskEverything = -1;

        // Paths to prefabs
        public const string GhostFullPath = "Prefabs/Ghost";
        public const string PacmanFullPath = "Prefabs/Pacman";              // Выпилить
        public const string PacmanTestFullPath = "Prefabs/PacmanTest";      // Выпилить
        public const string PacmanNewFullPath = "Prefabs/NewPacman";
        public const string NoFrictionMaterialFullPath = "Prefabs/NoFrictionMaterial";    // Материал без трения
        public const string WallTilesFolderPath = "Assets/WallTiles/";
        public const string FruitRuleTilesFolderPath = "Assets/FruitsRuleTiles/";
        public const string PelletRuleTilesFolderPath = "Assets/PelletsRuleTiles/";
        //public const string UIRootViewFullPath = "Prefabs/UI/RootViewUI";
        //public const string UIMainMenuFullPath = "Prefabs/UI/MainMenuUI";
        //public const string UIGameplayFullPath = "Prefabs/UI/GameplayUI";

        // Number of tiles along the specified path
        public const int NumberOfWallTiles = 38;
        public const int NumberOfPelletTiles = 3;
        public const int NumberOfFruitTiles = 1;         // Количество подбираемых вещей в папке Prefabs(apple, key и т.д.)

        // Map settings
        public const float GridCellSize = 1f;
        //public const int GridCellPixelSize = 24;
        public const int FieldsAtTheEdgesOfTheMap = 1;  // Отступы по краям карты

        //// Triggers object names
        //public const string TriggerPelletSmall = "PelletSmall(Clone)";
        //public const string TriggerPelletMedium = "PelletMedium(Clone)";
        //public const string TriggerPelletLarge = "PelletLarge(Clone)";
        //public const string TriggerFruit = "Fruit(Clone)";

        //// Score cost
        //public const int CostPelletSmall = 5;
        //public const int CostPelletMedium = 15;
        //public const int CostPelletLarge = 50;

        // Gameplay settings
        public const float PlayerSpeed = 6f;
        public const int StartLifePointsAmount = 3;
        public const int StartingDifficultyLevel = 1;

        // Level Constructor - переделать в enum?
        public const int EmptyTile = 0;
        public const int PacmanSpawn = -1;
        public const int SmallPellet = -3;
        public const int MediumPellet = -4;
        public const int LargePellet = -5;

        //// Errors
        //public const string PositionOnMapNotFound = "Position on map, not Found.";

        // UI and Camera
        public const float GameplayInformationalPamelHeight = 3f;

        // Signal Tag
        //public const string ExitSceneRequestTag = nameof(ExitSceneRequestTag);
    }
}