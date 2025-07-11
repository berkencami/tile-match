using Cysharp.Threading.Tasks;
using TileMatch.Tile;
using TileMatch.TileSystem;
using UnityEngine;
using DG.Tweening;
using TileMatch.Managers;

namespace TileMatch.Skill
{
    public class RemoveTileCommand : ICommand
    {
        private TileBarController _controller;
        private TileBase _tile;
        private int _removeIndex;
        private Transform _oldParent;
        private Vector3 _oldPosition;

        public RemoveTileCommand(TileBarController controller, TileBase tile, int removeIndex)
        {
            _controller = controller;
            _tile = tile;
            _removeIndex = removeIndex;
            _oldParent = tile.transform.parent;
            _oldPosition = tile.transform.position;
        }

        public async UniTask ExecuteAsync()
        {
            _controller.RemoveTileAt(_removeIndex);
            _tile.transform.SetParent(_oldParent);
            await _tile.transform.DOMove(_oldPosition, Game.VisualConfig.TileMoveDuration)
                .SetEase(Ease.OutBack)
                .AsyncWaitForCompletion();
            _tile.SetCollectedState(false);
        }

        public async UniTask UndoAsync()
        {
            _controller.AddTileAt(_tile, _removeIndex);
            _tile.SetCollectedState(true);
            await UniTask.Yield();
        }

        public TileBase GetTile()
        {
            return _tile;
        }
    }
} 