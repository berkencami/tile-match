using UnityEngine;

namespace TileMatch.Utility
{
    public class ScaleAdjuster : MonoBehaviour
    {
        [SerializeField] private Transform _tileBar;
        [SerializeField] private float _tileBarYOffset = 0f;
        
        private float _scaleFactor = 1f;
        private float _aspectRatio = 1F;
        private static float ReferenceRatio => 1.7f;

        private void Awake()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            
            _aspectRatio = screenWidth / screenHeight;
            _scaleFactor = Mathf.Clamp(_aspectRatio * ReferenceRatio, _aspectRatio, ReferenceRatio);
            transform.localScale = Vector3.one * _scaleFactor;

            // Adjust tile bar Y position based on scale factor
            if (_tileBar != null)
            {
                Vector3 tileBarPosition = _tileBar.localPosition;
                // Adjust Y position inversely to scale factor and add offset
                tileBarPosition.y = -2f * (1f - _scaleFactor) + _tileBarYOffset;
                _tileBar.localPosition = tileBarPosition;
            }
        }
    }
}