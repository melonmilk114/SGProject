namespace DontTouchTheSpikes
{
    public static class CommonFunc
    {
        public static WALL_DIR ConvertToPlayerMoveDirToWallDir(PLAYER_MOVE_DIR moveDir)
        {
            switch (moveDir)
            {
                case PLAYER_MOVE_DIR.LEFT:
                    return WALL_DIR.LEFT;
                case PLAYER_MOVE_DIR.RIGHT:
                    return WALL_DIR.RIGHT;
                default:
                    return WALL_DIR.NONE;
            }
        }
        
        public static WALL_DIR ReverseToWallDir(WALL_DIR moveDir)
        {
            switch (moveDir)
            {
                case WALL_DIR.LEFT:
                    return WALL_DIR.RIGHT;
                case WALL_DIR.RIGHT:
                    return WALL_DIR.LEFT;
                default:
                    return WALL_DIR.NONE;
            }
        }
    }
}