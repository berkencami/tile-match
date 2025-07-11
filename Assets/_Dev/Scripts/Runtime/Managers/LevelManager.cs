using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using TileMatch.Enums;
using TileMatch.LevelSystem;
using TileMatch.Tile;
using TileMatch.Utility;
using UnityEngine;

namespace TileMatch.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private Transform _tileParent;
        [SerializeField] private GameObject _tileBar;

        private readonly List<TileBase> _spawnedTiles = new List<TileBase>();
       

        private void Awake()
        {
            EventManager.OnLevelStateChange += OnLevelStateChange;
            EventManager.OnTileCollect += OnTileCollect;
            EventManager.OnUseSkill += SetInteraction;
            EventManager.OnMatch += CheckLevelSuccessCondition;
            InitializeLevel();
        }

        private void OnLevelStateChange(LevelState obj)
        {
            if(obj == LevelState.LevelWillLoad)
            {
                _tileBar.SetActive(true);
                SpawnLevel();
            }
        }

        private void InitializeLevel()
        {
            _tileBar.SetActive(false);
        }

        [Button]
        private void SpawnLevel()
        {
            DestroyLevel();
            SpawnLevelAsync().Forget();
        }

        private async UniTask SpawnLevelAsync()
        {
            var level = Game.LoadLevelAsync();
            await LevelLoader.LoadLevel(level, _tileParent);
            CacheSpawnedTiles();
            SetInteraction();
        }
        
        private void CacheSpawnedTiles()
        {
            _spawnedTiles.Clear();
            _spawnedTiles.AddRange(_tileParent.GetComponentsInChildren<TileBase>());
        }

        private void OnTileCollect(TileBase tileBase)
        {
            _spawnedTiles.Remove(tileBase);
            SetInteraction();
        }
        
        [Button]
        private void SetInteraction()
        {
            foreach (var tile in _spawnedTiles)
            {
                var pos = (Vector2)tile.transform.position;
                var colliders = Physics2D.OverlapPointAll(pos);

                var hasUpperTile = false;
              
                
                foreach (var col in colliders)
                {
                    var otherTile = col.GetComponent<TileBase>();
                    if (otherTile == null || otherTile == tile || otherTile.IsCollected) continue;
                    if (otherTile.SortingOrder <= tile.SortingOrder) continue;
                    hasUpperTile = true;
                    break;
                }

                tile.SetInteraction(!hasUpperTile);
                
            }
        }
        
        [Button]
        private void DestroyLevel()
        {
            var children = new List<GameObject>();
            foreach (Transform child in _tileParent)
            {
                children.Add(child.gameObject);
            }
            
            foreach (GameObject child in children)
            {
                DestroyImmediate(child);
            }
            
            _spawnedTiles.Clear();
        }
        
        private void CheckLevelSuccessCondition()
        {
            if(_spawnedTiles.Count > 0) return;
            EventManager.PublishLevelStateChangeEvent(LevelState.LevelSuccess);
        }
        
        public List<TileBase> BoardTiles => _spawnedTiles;
    }
}