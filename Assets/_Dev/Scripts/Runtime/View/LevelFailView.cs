using DG.Tweening;
using TileMatch.Enums;
using TileMatch.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace TileMatch.View
{
    public class LevelFailView : ViewBase
    {
        [SerializeField] private CanvasGroup _background;
        [SerializeField] private Button _button;
        private Transform _buttonTransform;
        private Vector3 _buttonInitialScale;

        private void Awake()
        {
            _button.onClick.AddListener(ButtonClicked);
            _buttonTransform = _button.transform;
            _buttonInitialScale = _buttonTransform.localScale;
        }

        protected override void OnShow()
        {
            InitializeAlpha();
            _buttonTransform.localScale = Vector3.zero;
            ViewOpenAction();
        }

        protected override void OnHide()
        {
        }

        private void ViewOpenAction()
        {
            _background.DOFade(Game.VisualConfig.ViewBackgroundAlpha,
                Game.VisualConfig.ViewFadeDuration).OnComplete(() =>
            {
                _buttonTransform.DOScale(_buttonInitialScale, Game.VisualConfig.ButtonScaleUpDuration)
                    .SetEase(Game.VisualConfig.ButtonScaleUpEase);
            });
        }

        private void InitializeAlpha()
        {
            _background.alpha = 0;
        }

        private void ButtonClicked()
        {
            EventManager.PublishLevelStateChangeEvent(LevelState.LevelInitialize);
            Hide();
        }
    }
}