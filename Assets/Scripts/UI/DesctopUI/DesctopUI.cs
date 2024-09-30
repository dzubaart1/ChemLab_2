using UnityEngine;

namespace BioEngineerLab.UI
{
    [RequireComponent(typeof(DesctopPanelSwitcher))]
    public class DesctopUI : MonoBehaviour
    {
        private DesctopPanelSwitcher _desctopPanelSwitcher;

        private void Awake()
        {
            _desctopPanelSwitcher = GetComponent<DesctopPanelSwitcher>();
        }
    }
}
