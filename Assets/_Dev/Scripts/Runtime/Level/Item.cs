using System;
using UnityEngine;

namespace TileMatch.LevelSystem
{
    [Serializable]
    public class Item
    {
        [SerializeField] private string _id;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Texture _icon;
        
        public string Id => _id;
        public GameObject Prefab => _prefab;
        public Texture Icon => _icon;
    }
}
