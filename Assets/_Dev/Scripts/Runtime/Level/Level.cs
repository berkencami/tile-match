using UnityEngine;
using System.Collections.Generic;

namespace TileMatch.LevelSystem
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 0)]
    public class Level : ScriptableObject
    {
        [SerializeField] private ItemPalette _itemPalette;
        [SerializeField] private int _layerCount = 1;
        [SerializeField] private List<LayerData> _layers = new List<LayerData>();
        
        public ItemPalette ItemPalette => _itemPalette;
        public int LayerCount => _layerCount;
        public List<LayerData> Layers => _layers;
    }

    [System.Serializable]
    public class LayerData
    {
        public int row;
        public int column;
        public string data;
    }
}