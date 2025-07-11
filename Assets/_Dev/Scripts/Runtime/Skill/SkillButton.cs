using UnityEngine;
using UnityEngine.UI;

namespace TileMatch.Skill
{
    [RequireComponent(typeof(Button))]
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] private SkillType skillType;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        private async void OnButtonClicked()
        {
            button.interactable = false;
            await SkillManager.Instance.UseSkillAsync(skillType);
            button.interactable = true;
        }
    }
} 