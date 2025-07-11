using System;
using TileMatch.Enums;
using TileMatch.Tile;

namespace TileMatch.Managers
{
    public static class EventManager
    {
        public static event Action<GameState> OnGameStateChange;
        private static GameState gameState;
        
        public static event Action<LevelState> OnLevelStateChange;
        private static LevelState levelState;

        public static event Action OnMatch;

        public static event Action<TileBase> OnTileCollect;

        public static event Action OnUseSkill; 
        
        public static void PublishGameStateChangeEvent(GameState newState)
        {
            if(gameState==newState) return;
            gameState = newState;
            OnGameStateChange?.Invoke(newState);
        }
        
        public static void PublishLevelStateChangeEvent(LevelState newState)
        {
            if(levelState==newState) return;
            levelState = newState;
            OnLevelStateChange?.Invoke(levelState);
        }

        public static void PublishMatchEvent()
        {
            OnMatch?.Invoke();
        }
        
        public static void PublishTileCollectEvent(TileBase tile)
        {
            OnTileCollect?.Invoke(tile);
        }

        public static void PublishUseSkillEvent()
        {
            OnUseSkill?.Invoke();
        }
    }
}