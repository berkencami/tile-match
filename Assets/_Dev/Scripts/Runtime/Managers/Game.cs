using System;
using TileMatch.Config;
using TileMatch.LevelSystem;
using UnityEngine;

namespace TileMatch.Managers
{
    public static class Game
    {
        public static VisualConfig VisualConfig { get; private set; } 
      
        public static ViewConfig ViewConfig { get; private set; }
      
        private static LevelConfig levelConfig;

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            VisualConfig = Resources.Load<VisualConfig>("Configs/VisualConfig");
            ViewConfig = Resources.Load<ViewConfig>("Configs/ViewConfig");
            levelConfig = Resources.Load<LevelConfig>("Configs/LevelConfig");
        }
        
        public static Level LoadLevelAsync()
        {
            GC.Collect();
            return levelConfig.GetLevel();
        }
    }
}