using System;
using TileMatch.Enums;
using TileMatch.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace TileMatch.UI
{
    public class PlayButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            EventManager.PublishLevelStateChangeEvent(LevelState.LevelInitialize);
        }
    }
}