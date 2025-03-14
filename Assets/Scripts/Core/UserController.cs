using UI.TabletUI;
using UnityEngine;

namespace Core
{
    public class UserController : MonoBehaviour
    {
        [SerializeField] private TabletUI _tabletUI;
        
        public void ToggleTabletUI()
        {
            _tabletUI.ToggleVisible();
        }
    }
}