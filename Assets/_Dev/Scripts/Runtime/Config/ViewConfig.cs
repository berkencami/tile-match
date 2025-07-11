using TileMatch.View;
using UnityEngine;

namespace TileMatch.Config
{
    [CreateAssetMenu(fileName = "ViewConfig", menuName = "ScriptableObjects/ViewConfig", order = 0)]
    public class ViewConfig : ScriptableObject
    {
        [SerializeField] private ViewBase[] _viewBases;
      
        public ViewBase GetView<T>() where T : ViewBase
        {
            T existing = FindObjectOfType<T>();
            if (existing != null)
                return existing;
            
            foreach (var viewBase in _viewBases)
            {
                if (viewBase is T instance)
                {
                    return Instantiate(instance);
                }
            }
            return null;
        }
    }
}

