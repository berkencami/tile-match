using UnityEngine;

namespace TileMatch.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private int _targetFrameRate = 60;
        
        public int TargetFrameRate => _targetFrameRate;
    }
}