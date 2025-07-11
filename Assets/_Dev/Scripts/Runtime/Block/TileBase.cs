using Cysharp.Threading.Tasks;
using DG.Tweening;
using TileMatch.Managers;
using TileMatch.ScriptableObjects.Config;
using UnityEngine;

namespace TileMatch.Tile
{
    public class TileBase : MonoBehaviour
    { 
        [SerializeField] private TileType _type;
        private bool _interaction;
        private bool _isCollected;
        private int _defaultSortingOrder;
        
        private SpriteRenderer _spriteRenderer;
        private Transform _transform;
        private Collider2D _collider2D;

        public int SortingOrder => _spriteRenderer.sortingOrder;
        public TileType Type => _type;
        public bool IsCollected => _isCollected;
        public bool Interaction => _interaction;
        
        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = GetComponent<Transform>();
            _isCollected = false;
            _interaction = true;
        }
        
        public void InitializeSortingOrder(int order)
        {
            _spriteRenderer.sortingOrder = order;
            _defaultSortingOrder = order;
        }

        public void SetDefaultSortingOrder()
        {
            _spriteRenderer.sortingOrder = _defaultSortingOrder;
        }
        
        public void SetInteraction(bool interaction)
        {
            _interaction = interaction;
            SetInteractionColor(interaction);
        }
        
        public void SetCollectedState(bool collected)
        {
            _collider2D.enabled = !collected;
            _isCollected = collected;
            _spriteRenderer.sortingOrder = Game.VisualConfig.MaxSortLayer;
        }
        
        public async UniTask Destroy()
        {
            await _transform.DOScale(Vector3.zero, Game.VisualConfig.MatchScaleDownDuration)
                .SetEase(Game.VisualConfig.MatchEase).OnComplete(() =>
                {
                    Destroy(gameObject);
                    FXManager.Instance.PlayParticle(ParticleType.Match, _transform.position, Quaternion.identity);
                    FXManager.Instance.PlaySoundFX(SoundType.Match);
                })
                .AsyncWaitForCompletion();
        }

        public void DestroyImmediately()
        {
            Destroy(gameObject);
        }

        public void Movement(Vector3 position)
        {
            _transform.DOMove(position, Game.VisualConfig.TileMoveDuration);
        }

        public async UniTask PlayMagnetPolishEffect()
        {
            _spriteRenderer.sortingOrder = Game.VisualConfig.MaxSortLayer;
            _spriteRenderer.color = Color.white;
            
            var sequence = DOTween.Sequence();
            
            sequence.Append(_transform.DOScale(Game.VisualConfig.MagnetScaleMultiplier, Game.VisualConfig.MagnetScaleUpDuration)
                .SetEase(Ease.OutBack));
            sequence.Append(_transform.DORotate(new Vector3(0, 0, Game.VisualConfig.MagnetShakeRotationAngle),
                Game.VisualConfig.MagnetShakeDuration).SetEase(Ease.InOutSine));
            sequence.Append(_transform.DORotate(new Vector3(0, 0, -Game.VisualConfig.MagnetShakeRotationAngle),
                Game.VisualConfig.MagnetShakeDuration).SetEase(Ease.InOutSine));

            sequence.OnComplete(() =>
            {
                _transform.rotation = Quaternion.Euler(0, 0, 0);
                _transform.localScale = Vector3.one;
            });
            await sequence.AsyncWaitForCompletion();
        }

        public async UniTask MoveTo(Vector3 position, int sortingOrder)
        {
            await _transform.DOMove(position, Game.VisualConfig.ShuffleDuration).SetEase(Game.VisualConfig.ShuffleEase).AsyncWaitForCompletion();
            _spriteRenderer.sortingOrder = sortingOrder;
            _defaultSortingOrder = sortingOrder;
        }

        private void SetInteractionColor(bool interaction)
        {
            var color = interaction ? Color.white : Game.VisualConfig.DisableTileColor;
            _spriteRenderer.DOKill();
            _spriteRenderer.DOColor(color, Game.VisualConfig.TileColorDuration);
        }
    }
}