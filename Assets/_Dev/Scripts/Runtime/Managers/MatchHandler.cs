using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TileMatch.Tile;

namespace TileMatch.Managers
{
    public class MatchHandler 
    {
        public async UniTask Match(List<TileBase> tiles)
        {
            var asyncOperations = new List<UniTask>();
            foreach (var tileBase in tiles)
            {
                asyncOperations.Add(tileBase.Destroy());
            }

            await UniTask.WhenAll(asyncOperations);
        }
    }
}