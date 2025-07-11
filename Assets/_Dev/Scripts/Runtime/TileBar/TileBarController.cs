using UnityEngine;
using System.Collections.Generic;
using TileMatch.Tile;
using TileMatch.Utility;
using Cysharp.Threading.Tasks;
using TileMatch.Enums;
using TileMatch.Managers;
using TileMatch.ScriptableObjects.Config;
using TileMatch.Skill;

namespace TileMatch.TileSystem
{
    public class TileBarController : Singleton<TileBarController>
    {
        [Header("Bar Settings")]
        [SerializeField] private TileSlot[] _barSlots;
        [SerializeField] private List<TileBase> _attachedTiles;
        private MatchHandler _matchHandler;
        private Stack<ICommand> _commandStack = new Stack<ICommand>();
        public Stack<ICommand> CommandStack => _commandStack;
        
        public List<TileBase> AttachedTiles => _attachedTiles;
        
        public TileSlot[] BarSlots => _barSlots;

        #region Unity Lifecycle
        private void Awake()
        {
            _matchHandler = new MatchHandler();
            EventManager.OnLevelStateChange += OnGameStateChange;
        }

        private void OnDestroy()
        {
            EventManager.OnLevelStateChange -= OnGameStateChange;
        }
        #endregion

        #region Public Methods
        public async void RequestFillSlot(TileBase tile)
        {
            if (!CanAcceptNewTile()) return;
            FXManager.Instance.PlaySoundFX(SoundType.Collect);
            tile.SetCollectedState(true);
            var matchedSlotIndex = FindMatchingSlotIndex(tile);
            var insertIndex = matchedSlotIndex != -1 ? matchedSlotIndex + 1 : _attachedTiles.Count;
            var command = new AddTileCommand(this, tile, insertIndex);
            _commandStack.Push(command);
            await command.ExecuteAsync();
            CheckGameOverCondition();
            EventManager.PublishTileCollectEvent(tile);
        }

        public async void RequestReverseMove()
        {
            if (!CanReverseMove()) return;
            var lastTile = _attachedTiles[^1];
            var command = new RemoveTileCommand(this, lastTile, _attachedTiles.Count - 1);
            _commandStack.Push(command);
            await command.ExecuteAsync();
        }

        public void AddTileAt(TileBase tile, int index)
        {
            if (index < 0 || index > _attachedTiles.Count) return;
            _attachedTiles.Insert(index, tile);
            _barSlots[index].Fill(tile);
            ReorderSlotElements().Forget();
        }

        public void RemoveTile(TileBase tile)
        {
            var index = _attachedTiles.IndexOf(tile);
            if (index < 0) return;
            _barSlots[index].Clear();
            _attachedTiles.RemoveAt(index);
            ReorderSlotElements().Forget();
        }

        public void RemoveTileAt(int index)
        {
            if (index < 0 || index >= _attachedTiles.Count) return;
            _barSlots[index].Clear();
            _attachedTiles.RemoveAt(index);
            ReorderSlotElements().Forget();
        }
        #endregion

        #region Private Methods
        private void OnGameStateChange(LevelState state)
        {
            if (state == LevelState.LevelInitialize)
            {
                ClearAllSlots();
            }
        }

        private bool CanAcceptNewTile()
        {
            return _attachedTiles.Count < _barSlots.Length;
        }

        private bool CanReverseMove()
        {
            if (_attachedTiles.Count == 0)
            {
                Debug.LogWarning("Cannot reverse move: Bar is empty!");
                return false;
            }
            return true;
        }

        private int FindMatchingSlotIndex(TileBase tile)
        {
            var lastMatchIndex = -1;
            for (var i = 0; i < _attachedTiles.Count; i++)
            {
                if (_attachedTiles[i].Type.Equals(tile.Type))
                {
                    lastMatchIndex = i;
                }
            }
            return lastMatchIndex;
        }

        private void CheckGameOverCondition()
        {
            if (_attachedTiles.Count == _barSlots.Length && !HasAnyValidMatch(out _))
            {
                EventManager.PublishLevelStateChangeEvent(LevelState.LevelFailed);
            }
        }

        private async UniTaskVoid ReorderSlotElements()
        {
            for (var i = 0; i < _barSlots.Length; i++)
            {
                _barSlots[i].Clear();
                if (i < _attachedTiles.Count)
                {
                    _barSlots[i].Fill(_attachedTiles[i]);
                }
            }

            await UniTask.Delay(Game.VisualConfig.MatchDelayMS);
            ValidateSlots();
            
        }

        private bool HasAnyValidMatch(out int matchStartIndex)
        {
            matchStartIndex = -1;
            if (_attachedTiles.Count < Game.VisualConfig.MinMatchCount) return false;

            for (var i = 0; i <= _attachedTiles.Count - Game.VisualConfig.MinMatchCount; i++)
            {
                if (AreConsecutiveTilesMatching(i))
                {
                    matchStartIndex = i;
                    return true;
                }
            }
            return false;
        }

        private bool AreConsecutiveTilesMatching(int startIndex)
        {
            return _attachedTiles[startIndex].Type == _attachedTiles[startIndex + 1].Type &&
                   _attachedTiles[startIndex].Type == _attachedTiles[startIndex + 2].Type;
        }

        private void ValidateSlots()
        {
            if (!HasAnyValidMatch(out var matchStartIndex)) return;
            
            MatchSlots(matchStartIndex).Forget();
        }

        private void CleanCommandsForMatchedTiles(List<TileBase> matchedTiles)
        {
            if (_commandStack.Count == 0) return;
            var newStack = new Stack<ICommand>();
            while (_commandStack.Count > 0)
            {
                var cmd = _commandStack.Pop();
                var tile = GetTileFromCommand(cmd);
                if (tile != null && matchedTiles.Contains(tile))
                    continue;
                newStack.Push(cmd);
            }
            while (newStack.Count > 0)
                _commandStack.Push(newStack.Pop());
        }

        private TileBase GetTileFromCommand(ICommand command)
        {
            // Safe way to get tile from command without reflection
            if (command is AddTileCommand addCommand)
            {
                return addCommand.GetTile();
            }
            else if (command is RemoveTileCommand removeCommand)
            {
                return removeCommand.GetTile();
            }
            return null;
        }

        private async UniTaskVoid MatchSlots(int startingIndex)
        {
            var matchedTiles = new List<TileBase>();
            for (var i = startingIndex + 2; i >= startingIndex; i--)
            {
                var targetTile = _attachedTiles[i];
                matchedTiles.Add(targetTile);
                RemoveTileFromSlot(_barSlots[i], targetTile);
            }

            CleanCommandsForMatchedTiles(matchedTiles);
            await _matchHandler.Match(matchedTiles);
            EventManager.PublishMatchEvent();
            ReorderSlotElements().Forget();
        }

        private void RemoveTileFromSlot(TileSlot targetSlot, TileBase tile)
        {
            targetSlot.Clear();
            _attachedTiles.Remove(tile);
        }

        private void ClearAllSlots()
        {
            foreach (var slot in _barSlots)
            {
                if (slot.IsEmpty) continue;
                slot.Tile.DestroyImmediately();
                slot.Clear();
            }
            _attachedTiles.Clear();
        }
        #endregion
    }
}