using System;
using UnityEngine;

namespace UI
{
    public abstract class BasePanel<T> : MonoBehaviour where T : Enum
    {
        public event Action<T> SwitchPanelRequest; 
        
        public T PanelType;
        
        public virtual void ShowPanel()
        {
            gameObject.SetActive(true);
        }

        public virtual void HidePanel()
        {
            gameObject.SetActive(false);
        }

        public virtual void SwitchPanel(T panel)
        {
            SwitchPanelRequest?.Invoke(panel);
        }
    }
}