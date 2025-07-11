using DG.Tweening;
using UnityEngine;

namespace TileMatch.Config
{
    [CreateAssetMenu(fileName = "VisualConfig", menuName = "ScriptableObjects/VisualConfig", order = 0)]
    public class VisualConfig : ScriptableObject
    {
        [SerializeField] private int _maxSortLayer = 32767;
        public int MaxSortLayer => _maxSortLayer;

        [Space(10)]

        #region LevelConfig

        [Header("Level Config")]
        [SerializeField]
        private float _tileSize = 0.77f;

        [SerializeField] private float _spawnHeight = 1f;
        [SerializeField] private float _spawnDuration = 0.86f;
        [SerializeField] private Ease _spawnEase = Ease.OutBack;
        [SerializeField] private float _rowMoveDelay = 0.1f;
        [SerializeField] private float _tileMoveDelay = 0.05f;
        [SerializeField] private Color _disableTileColor = Color.black;
        [SerializeField] private float _tileColorDuration = 0.2f;
        public float SpawnHeight => _spawnHeight;
        public float SpawnDuration => _spawnDuration;
        public Ease SpawnEase => _spawnEase;
        public float RowMoveDelay => _rowMoveDelay;
        public float TileMoveDelay => _tileMoveDelay;
        public float TileSize => _tileSize;
        public Color DisableTileColor => _disableTileColor;
        
        public float TileColorDuration => _tileColorDuration;

        #endregion

        [Space(10)]

        #region MatchConfig

        [Header("Match Config")]
        [SerializeField]
        private float _matchScaleDownDuration = 0.5f;

        [SerializeField] private Ease _matchEase = Ease.OutBack;

        public float MatchScaleDownDuration => _matchScaleDownDuration;
        public Ease MatchEase => _matchEase;

        #endregion


        [Space(10)]

        #region TileMovement

        [Header("Tile Movement")]
        [SerializeField]
        private float _tileMoveDuration = 0.5f;

        public float TileMoveDuration => _tileMoveDuration;

        [SerializeField] private Ease _undoMovementEase = Ease.Linear;

        public Ease UndoMovementEase => _undoMovementEase;

        #endregion

        [Space(10)]

        #region GameConfig

        [Header("Game Config")]
        [SerializeField]
        private int _minMatchCount = 3;

        [SerializeField] private int _matchDelayMS = 300;
        [SerializeField] private int _destroyDelayMS = 350;

        public int MinMatchCount => _minMatchCount;
        public int MatchDelayMS => _matchDelayMS;
        public int DestroyDelayMS => _destroyDelayMS;

        #endregion

        [Space(10)]

        #region UI

        [Header("UI")]
        [SerializeField]
        private float _viewBackgroundAlpha = 0.8f;

        [SerializeField] private float _viewFadeDuration = 0.5f;
        [SerializeField] private float _buttonScaleUpDuration = 0.5f;
        [SerializeField] private Ease _buttonScaleUpEase = Ease.OutBack;
        

        public float ViewBackgroundAlpha => _viewBackgroundAlpha;
        public float ViewFadeDuration => _viewFadeDuration;
        public float ButtonScaleUpDuration => _buttonScaleUpDuration;
        public Ease ButtonScaleUpEase => _buttonScaleUpEase;

        #endregion

        #region Skill

        [Space(10)] [Header("Skill")] [SerializeField]
        private float _magnetScaleMultiplier = 1.2f;

        public float MagnetScaleMultiplier => _magnetScaleMultiplier;

        [SerializeField] private float _magnetScaleUpDuration = 0.15f;
        public float MagnetScaleUpDuration => _magnetScaleUpDuration;

        [SerializeField] private float _magnetShakeRotationAngle = 20f;
        public float MagnetShakeRotationAngle => _magnetShakeRotationAngle;
        [SerializeField] private float _magnetShakeDuration = 0.15f;
        public float MagnetShakeDuration => _magnetShakeDuration;


        [SerializeField] private float _shuffleDuration = 0.5f;
        public float ShuffleDuration => _shuffleDuration;

        [SerializeField] private Ease _shuffleEase = Ease.InOutBack;
        public Ease ShuffleEase => _shuffleEase;

        #endregion
    }
}