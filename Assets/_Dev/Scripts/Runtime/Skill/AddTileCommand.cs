using Cysharp.Threading.Tasks;
using TileMatch.Tile;
using TileMatch.TileSystem;
using UnityEngine;
using DG.Tweening;
using TileMatch.Managers;

namespace TileMatch.Skill
{
    public class AddTileCommand : ICommand
    {
        private TileBarController _controller;
        private TileBase _tile;
        private int _insertIndex;
        private Vector3 _oldPosition;

        public AddTileCommand(TileBarController controller, TileBase tile, int insertIndex)
        {
            _controller = controller;
            _tile = tile;
            _insertIndex = insertIndex;
            _oldPosition = tile.transform.position;
        }

        public async UniTask ExecuteAsync()
        {
            _controller.AddTileAt(_tile, _insertIndex);
            await UniTask.Yield();
        }

        public async UniTask UndoAsync()
        {
            _controller.RemoveTile(_tile);
            await _tile.transform.DOMove(_oldPosition, Game.VisualConfig.TileMoveDuration)
                .SetEase(Game.VisualConfig.UndoMovementEase)
                .AsyncWaitForCompletion();
            _tile.SetCollectedState(false);
            
            EventManager.PublishUseSkillEvent();
        }

        public TileBase GetTile()
        {
            return _tile;
        }
    }
} 