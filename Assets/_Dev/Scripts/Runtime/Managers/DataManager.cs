using TileMatch.Enums;
using UnityEngine;

namespace TileMatch.Managers
{
    public static class DataManager
    {
        private const string LevelIndexKey = "LevelIndex";

        [RuntimeInitializeOnLoadMethod]
        public static void ListenGameEvents()
        {
            EventManager.OnLevelStateChange += OnLevelStateChange;
        }

        private static void OnLevelStateChange(LevelState obj)
        {
            if (obj != LevelState.LevelSuccess) return;
            IncrementLevelIndex();
        }

        public static int GetLevelIndex()
        {
            var levelIndex = PlayerPrefs.GetInt(LevelIndexKey, 0);
            return levelIndex;
        }

        private static void IncrementLevelIndex()
        {
            var levelIndex = GetLevelIndex();
            levelIndex++;
            PlayerPrefs.SetInt(LevelIndexKey, levelIndex);
            PlayerPrefs.Save();
        }
        
    }
}