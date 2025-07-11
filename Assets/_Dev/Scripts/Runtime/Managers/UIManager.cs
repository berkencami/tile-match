using TileMatch.Enums;
using TileMatch.View;
using System.Collections.Generic;
using TileMatch.Utility;
using UnityEngine;

namespace TileMatch.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        private static Dictionary<System.Type, ViewBase> _spawnedViews = new Dictionary<System.Type, ViewBase>();

        private void Awake()
        {
            EventManager.OnLevelStateChange += OnLevelStateChange;
            EventManager.OnGameStateChange += OnGameStateChange;
        }

        private void OnLevelStateChange(LevelState obj)
        {
            switch (obj)
            {
                case LevelState.LevelInitialize:
                    GetView<LevelTransition>().Show();
                    break;
                case LevelState.LevelSuccess:
                    GetView<LevelSuccessView>().Show();
                    break;
                case LevelState.LevelFailed:
                    GetView<LevelFailView>().Show();
                    break;
                case LevelState.Invalid:
                    break;
                case LevelState.LevelWillLoad:
                    InitializeInGameUI();
                    break;
                case LevelState.LevelDidLoad:
                    break;
                case LevelState.LevelReadyToPlay:
                    break;
                default:
                    break;
            }
        }
        
        private void OnGameStateChange(GameState obj)
        {
            if (obj == GameState.GameStart)
            {
                InitializeMenuUI();
            }
        }

        private void InitializeInGameUI()
        {
            GetView<MenuView>().Hide();
            GetView<InGameView>().Show();
        }

        private void InitializeMenuUI()
        {
            GetView<MenuView>().Show();
        }

        private T GetView<T>() where T : ViewBase
        {
            var type = typeof(T);
            if (_spawnedViews.TryGetValue(type, out var view))
            {
                return view as T;
            }

            var spawned = Game.ViewConfig.GetView<T>() as T;
            if (spawned != null)
            {
                _spawnedViews[type] = spawned;
            }

            return spawned;
        }
    }
}