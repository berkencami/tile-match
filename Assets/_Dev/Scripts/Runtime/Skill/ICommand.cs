using Cysharp.Threading.Tasks;

namespace TileMatch.Skill
{
    public interface ICommand
    {
        UniTask ExecuteAsync();
        UniTask UndoAsync();
    }
} 