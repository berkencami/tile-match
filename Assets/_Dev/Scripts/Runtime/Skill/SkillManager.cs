using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TileMatch.Utility;

namespace TileMatch.Skill
{
    public class SkillManager : Singleton<SkillManager>
    {
        private Dictionary<SkillType, ISkill> _skills;

        private void Awake()
        {
            _skills = new Dictionary<SkillType, ISkill>
            {
                { SkillType.Undo, new UndoSkill() },
                { SkillType.Magnet, new MagnetSkill() },
                { SkillType.Shuffle, new ShuffleSkill() }
            };
        }

        public async UniTask UseSkillAsync(SkillType type)
        {
            if (_skills.TryGetValue(type, out var skill))
            {
                if (skill.CanActivate() && !skill.IsActive)
                {
                    await skill.ActivateAsync();
                }
            }
        }
    }
} 