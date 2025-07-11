using TileMatch.Managers;
using TMPro;
using UnityEngine;

namespace TileMatch.UI
{
    public class LevelText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text= GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            SetLevelText();
        }

        public void SetLevelText()
        {
            _text.text = $"Level {(DataManager.GetLevelIndex() + 1)}";
        }
    }
}
