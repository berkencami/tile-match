using Cysharp.Threading.Tasks;
using TileMatch.TileSystem;
using UnityEngine;

namespace TileMatch.Skill
{
    public class UndoSkill : ISkill
    {
        public bool IsActive { get; private set; }

        public bool CanActivate()
        {
            return TileBarController.Instance.CommandStack.Count > 0;
        }

        public async UniTask ActivateAsync()
        {
            if (!CanActivate()) return;
            IsActive = true;
            var stack = TileBarController.Instance.CommandStack;
            var command = stack.Pop();
            await command.UndoAsync();
            IsActive = false;
        }
    }
} 