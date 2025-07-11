using System.Linq;
using TileMatch.Utility;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using TileMatch.Enums;
using TileMatch.Managers;
using TileMatch.Tile;

namespace TileMatch.LevelSystem
{
    public static class LevelLoader
    {
        
        public static async UniTask LoadLevel(Level level, Transform parent, CancellationToken cancellationToken = default)
        {
            if (level == null || level.Layers == null || level.Layers.Count == 0)
            {
                Debug.LogError("Level or layer data not found!");
                return;
            }
            
            var maxRow = 1;
            var maxCol = 1;
            foreach (var layer in level.Layers)
            {
                if (layer.row > maxRow) maxRow = layer.row;
                if (layer.column > maxCol) maxCol = layer.column;
            }
            
            var allTiles = new List<TileBase>();
            var allTilesByRow = new List<Dictionary<int, List<(GameObject instance, Vector3 targetPosition)>>>();
            
            for (var layerIndex = 0; layerIndex < level.Layers.Count; layerIndex++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var layer = level.Layers[layerIndex];
                var layerParent = new GameObject($"Layer_{layerIndex}").transform;
                layerParent.SetParent(parent);
                layerParent.localPosition = Vector3.zero;
                var layerData = JsonExtension.GetFromJson<LevelData>(layer.data);
                if (layerData == null || layerData.Length == 0)
                {
                    Debug.LogWarning($"No data found for Layer {layerIndex}");
                    continue;
                }
                var row = layer.row;
                var col = layer.column;
                var gridWidth = row * Game.VisualConfig.TileSize;
                var gridHeight = col * Game.VisualConfig.TileSize;
                var gridOrigin = new Vector3(
                    -gridWidth / 2f + Game.VisualConfig.TileSize / 2f,
                    gridHeight / 2f - Game.VisualConfig.TileSize / 2f,
                    layerIndex * -0.1f
                );
                
                var tilesByRow = new Dictionary<int, List<(GameObject instance, Vector3 targetPosition)>>();
                foreach (var dataItem in layerData)
                {
                    if (string.IsNullOrEmpty(dataItem._itemID)) continue;
                    var paletteItem = level.ItemPalette.Items.FirstOrDefault(fd => fd.Id == dataItem._itemID);
                    if (paletteItem == null || paletteItem.Prefab == null)
                    {
                        Debug.LogWarning($"Item not found: {dataItem._itemID}");
                        continue;
                    }
                    var posX = dataItem._row * Game.VisualConfig.TileSize;
                    var posY = -dataItem._column * Game.VisualConfig.TileSize;
                    var itemPosition = gridOrigin + new Vector3(posX, posY, 0);
                    
                    var spawnPosition = itemPosition + Vector3.up * Game.VisualConfig.SpawnHeight;
                    var instance = Object.Instantiate(paletteItem.Prefab, layerParent);
                    instance.transform.localPosition = spawnPosition;
                    instance.transform.localScale = Vector3.one;
                    
                    var tileBase = instance.GetComponent<TileBase>();
                    if (tileBase != null)
                    {
                        tileBase.InitializeSortingOrder(layerIndex + 1);
                        allTiles.Add(tileBase);
                    }
                    
                    if (!tilesByRow.ContainsKey(dataItem._column))
                    {
                        tilesByRow[dataItem._column] = new List<(GameObject instance, Vector3 targetPosition)>();
                    }
                    tilesByRow[dataItem._column].Add((instance, itemPosition));
                }
                
                allTilesByRow.Add(tilesByRow);
            }
               
            EventManager.PublishLevelStateChangeEvent(LevelState.LevelDidLoad);
            
            await AnimateTilesDown(allTilesByRow, cancellationToken);
           
            EventManager.PublishLevelStateChangeEvent(LevelState.LevelReadyToPlay);
        }
        
        private static async UniTask AnimateTilesDown(List<Dictionary<int, List<(GameObject instance, Vector3 targetPosition)>>> allTilesByRow, CancellationToken cancellationToken = default)
        {
            var allAnimations = new List<Tween>();
            
            foreach (var tilesByRow in allTilesByRow)
            {
                var sortedRows = tilesByRow.Keys.OrderByDescending(x => x).ToList();
                
                for (var rowIndex = 0; rowIndex < sortedRows.Count; rowIndex++)
                {
                    var sortedRow = sortedRows[rowIndex];
                    var tiles = tilesByRow[sortedRow];
                    
                    for (var i = 0; i < tiles.Count; i++)
                    {
                        var (instance, targetPosition) = tiles[i];
                        var tween = instance.transform
                            .DOLocalMove(targetPosition, Game.VisualConfig.SpawnDuration)
                            .SetEase(Game.VisualConfig.SpawnEase)
                            .SetDelay(rowIndex * Game.VisualConfig.RowMoveDelay +
                                      i * Game.VisualConfig.TileMoveDelay).SetAutoKill(false);
                        allAnimations.Add(tween);
                    }
                }
            }
            
            await UniTask.WaitUntil(() => allAnimations.All(tween => !tween.IsPlaying()), cancellationToken: cancellationToken);
        }
    }
}