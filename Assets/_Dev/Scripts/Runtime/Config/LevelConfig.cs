using TileMatch.LevelSystem;
using TileMatch.Managers;
using UnityEngine;

namespace TileMatch.Config
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private Level[] _levels;

        public Level GetLevel()
        {
            var levelIndex = DataManager.GetLevelIndex() % _levels.Length;
            return _levels[levelIndex];
        }
    }
}