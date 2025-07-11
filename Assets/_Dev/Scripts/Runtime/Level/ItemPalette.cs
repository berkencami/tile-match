using System.Collections.Generic;
using UnityEngine;

namespace TileMatch.LevelSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemPalette", fileName = "ItemPalette")]
    public class ItemPalette : ScriptableObject
    {
        [SerializeField] private List<Item> _items;
        public List<Item> Items => _items;
    }
}