using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TileMatch.Managers;
using UnityEngine;

namespace TileMatch.Skill
{
    public struct TileTarget
    {
        public Vector3 Position;
        public readonly int SortingOrder;
        public TileTarget(Vector3 position, int sortingOrder)
        {
            Position = position;
            SortingOrder = sortingOrder;
        }
    }
    
    public class ShuffleSkill : ISkill
    {
        public bool IsActive { get; private set; }

        public bool CanActivate()
        {
            return true;
        }

        public async UniTask ActivateAsync()
        {
            IsActive = true;
            var tiles = LevelManager.Instance.BoardTiles;
            var targets = new List<TileTarget>();
            foreach (var tile in tiles)
            {
                targets.Add(new TileTarget(tile.transform.position, tile.SortingOrder));
            }
            var rng = new System.Random();
            var n = targets.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (targets[k], targets[n]) = (targets[n], targets[k]);
            }
            var moveTasks = new List<UniTask>();
            for (var i = 0; i < tiles.Count; i++)
            {
                moveTasks.Add(tiles[i].MoveTo(targets[i].Position, targets[i].SortingOrder));
            }
            await UniTask.WhenAll(moveTasks);
            await UniTask.Yield();
            EventManager.PublishUseSkillEvent();
            IsActive = false;
        }
    }
}