using System;
using TileMatch.Enums;
using UnityEngine;
using TileMatch.Tile;
using TileMatch.TileSystem;
using TileMatch.Utility;

namespace TileMatch.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private LayerMask _tileLayer;
        [SerializeField] private Camera _mainCamera;
        private bool _inputEnabled = false;

        private void Awake()
        {
            EventManager.OnLevelStateChange += OnLevelStateChange;
        }

        private void OnLevelStateChange(LevelState obj)
        {
            _inputEnabled = obj == LevelState.LevelReadyToPlay;
        }

        private void Start()
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }
        }

        private void Update()
        {
            if(!_inputEnabled) return;
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseClick();
            }
        }

        private void HandleMouseClick()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _tileLayer);

            if (hit.collider == null) return;
            var tile = hit.collider.GetComponent<TileBase>();
            
            if (tile == null) return;

            if(tile.IsCollected || !tile.Interaction) return;
            TileBarController.Instance.RequestFillSlot(tile);
        }
    }
} 