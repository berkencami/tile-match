using TileMatch.Tile;
using UnityEngine;

namespace TileMatch.TileSystem
{
    public class TileSlot : MonoBehaviour
    {
        private Transform _transform;
        public TileBase Tile { get; private set; }

        public bool IsEmpty => Tile == null;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        public void Fill(TileBase newTile)
        {
            newTile.Movement(_transform.position);
            Tile = newTile;
        }

        public void Clear()
        {
            Tile = null;
        }
    }
}