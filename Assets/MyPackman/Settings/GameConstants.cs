namespace Assets.MyPackman.Settings
{
    public static class GameConstants
    {
        // Map
        public const float GridCellSize = 1f;

        // Packman
        public const int NoDirection = -1;
        public const int LeftDirection = 0;
        public const int RightDirection = 1;
        public const int UpDirection = 2;
        public const int DownDirection = 3;
        public const int MovementStep = 1;
        public const float PlayerSpeed = 0.05f;

        //Level Constructor - переделать в enum?
        public const int EmptyTile = -1;
        public const int Player = 0;
        public const int UpperLeftCornerWall = 176;
        public const int UpperBranchWall = 177;
        public const int UpperRightCornerWall = 178;
        public const int LeftBranchWall = 193;
        public const int CentralBranchWall = 194;
        public const int RigthBranchWall = 195;
        public const int LowerLeftCornerWall = 210;
        public const int LowerBranchWall = 211;
        public const int LowerRightCornerWall = 212;
        public const int DownEndWall = 213;
        public const int UpEndWall = 214;
        public const int HorizontalWall = 228;
        public const int VerticalWall = 227;
        public const int SingleWall = 229;
        public const int LeftEndWall = 230;
        public const int RightEndWall = 231;
        public const int Point = 336;

        // Errors
        public const string PositionOnMapNotFound = "Position on map, not Found.";
    }
}
