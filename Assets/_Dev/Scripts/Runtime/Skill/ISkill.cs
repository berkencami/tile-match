using Cysharp.Threading.Tasks;

namespace TileMatch.Skill
{
    public interface ISkill
    {
        bool CanActivate();
        UniTask ActivateAsync();
        bool IsActive { get; }
    }
} 