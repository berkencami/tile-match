using UnityEngine;

namespace TileMatch.View
{
    public abstract class ViewBase : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }
      
        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }
        
        protected abstract void OnShow();

        protected abstract void OnHide();
    }

}
