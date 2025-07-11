using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TileMatch.Managers;
using TileMatch.Tile;
using TileMatch.TileSystem;
using UnityEngine;

namespace TileMatch.Skill
{
    public class MagnetSkill : ISkill
    {
        public bool IsActive { get; private set; }

        public bool CanActivate()
        {
            var bar = TileBarController.Instance;
            var boardTiles = LevelManager.Instance.BoardTiles;
            var barTiles = bar.AttachedTiles;
            var barSlots = bar.BarSlots.Length;
            var emptySlots = barSlots - barTiles.Count;
            
            var availableTiles = boardTiles.Where(t => !t.IsCollected).ToList();
            
            if (barTiles.Count == 0)
            {
                var group = availableTiles.GroupBy(t => t.Type).FirstOrDefault(g => g.Count() >= 3);
                return group != null && emptySlots >= 3;
            }
            
            var twoOfAKind = barTiles.GroupBy(t => t.Type).FirstOrDefault(g => g.Count() == 2);
            if (twoOfAKind != null)
            {
                var type = twoOfAKind.Key;
                var boardCount = availableTiles.Count(t => t.Type == type);
                if (boardCount >= 1 && emptySlots >= 1)
                    return true;
            }

            if (emptySlots < 2) return false;
            {
                var singleTiles = barTiles.GroupBy(t => t.Type).Where(g => g.Count() == 1).ToList();
                foreach (var singleGroup in singleTiles)
                {
                    var type = singleGroup.Key;
                    var boardCount = availableTiles.Count(t => t.Type == type);
                    if (boardCount >= 2)
                        return true;
                }
            }

            return false;
        }

        public async UniTask ActivateAsync()
        {
            IsActive = true;
            var bar = TileBarController.Instance;
            var boardTiles = LevelManager.Instance.BoardTiles;
            var barTiles = bar.AttachedTiles;
            var barSlots = bar.BarSlots.Length;
            var emptySlots = barSlots - barTiles.Count;
            var toCollect = new List<TileBase>();
            
            var availableTiles = boardTiles.Where(t => !t.IsCollected).ToList();
            
            if (barTiles.Count == 0)
            {
                var group = availableTiles.GroupBy(t => t.Type).FirstOrDefault(g => g.Count() >= 3);
                if (group != null && emptySlots >= 3)
                {
                    toCollect = group.Take(3).ToList();
                }
            }
            else
            {
                var twoOfAKind = barTiles.GroupBy(t => t.Type).FirstOrDefault(g => g.Count() == 2);
                if (twoOfAKind != null)
                {
                    var type = twoOfAKind.Key;
                    var tile = availableTiles.FirstOrDefault(t => t.Type == type);
                    if (tile != null && emptySlots >= 1)
                    {
                        toCollect.Add(tile);
                    }
                }
                else if (emptySlots >= 2)
                {
                    var singleTiles = barTiles.GroupBy(t => t.Type).Where(g => g.Count() == 1).ToList();
                    var bestMatch = FindBestMatch(singleTiles, availableTiles);
                    if (bestMatch != null)
                    {
                        toCollect.AddRange(bestMatch);
                    }
                }
            }
            
            if (toCollect.Count == 0)
            {
                IsActive = false;
                return;
            }
            
            var polishTasks = new List<UniTask>();
            var barTargetPositions = new List<Vector3>();
            for (var i = 0; i < toCollect.Count; i++)
            {
                var tile = toCollect[i];
                var slotIndex = bar.AttachedTiles.Count + i;
                var slot = bar.BarSlots[slotIndex];
                var barTargetPos = slot.transform.position;
                barTargetPositions.Add(barTargetPos);
                polishTasks.Add(tile.PlayMagnetPolishEffect());
            }
            await UniTask.WhenAll(polishTasks);
            
            foreach (var tileBase in toCollect)
            {
                bar.RequestFillSlot(tileBase);
            }
            
            IsActive = false;
        }

        private List<TileBase> FindBestMatch(List<IGrouping<TileType, TileBase>> singleTiles, List<TileBase> interactableBoardTiles)
        {
            var bestType = singleTiles
                .OrderByDescending(single => interactableBoardTiles.Count(t => t.Type == single.Key))
                .FirstOrDefault();

            if (bestType == null) return null;
            {
                var type = bestType.Key;
                var availableTiles = interactableBoardTiles.Where(t => t.Type == type).Take(2).ToList();
                if (availableTiles.Count == 2)
                {
                    return availableTiles;
                }
            }

            return null;
        }
    }
} 