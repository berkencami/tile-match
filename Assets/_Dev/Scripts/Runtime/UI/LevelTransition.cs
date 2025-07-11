using Cysharp.Threading.Tasks;
using DG.Tweening;
using TileMatch.Enums;
using TileMatch.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace TileMatch.View
{
    public class LevelTransition : ViewBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Awake()
        {
            EventManager.OnLevelStateChange += OnLevelStateChange;
        }

        private void OnLevelStateChange(LevelState obj)
        {
            if (obj == LevelState.LevelDidLoad)
            {
                EndTransition();
            }
        }

        protected override void OnShow()
        {
            _canvasGroup.alpha = 0;
            StartTransitionAsync().Forget();
        }
        
        private async UniTaskVoid StartTransitionAsync()
        {
            await _canvasGroup.DOFade(1, .7f).AsyncWaitForCompletion();
            EventManager.PublishLevelStateChangeEvent(LevelState.LevelWillLoad);
        }

        protected override void OnHide()
        {
        }

        private void EndTransition()
        {
            _canvasGroup.alpha = 0;
            Hide();
        }
    }
}

