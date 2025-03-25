using System;
using UI.TabletUI;
using UnityEngine;

namespace Core
{
    public class UserController : MonoBehaviour
    {
        public event Action ButtonClicked;
        [SerializeField] private TabletUI _tabletUI;
        
        public void ToggleTabletUI()
        {
            _tabletUI.ToggleVisible();
        }

        public void ToggleSubstanceUI()
        {
            ButtonClicked?.Invoke();
        }
    }
}