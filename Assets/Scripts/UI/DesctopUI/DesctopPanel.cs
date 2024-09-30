using BioEngineerLab.UI.Components;
using UnityEngine;

namespace BioEngineerLab.UI
{
    public class DesctopPanel : BasePanel<DesctopPanelsType>
    {
        [SerializeField] private ButtonComponent _applicationButton;

        private void Awake()
        {
            _applicationButton.OnClickButton += OnClickApplicationBtn;
        }

        private void OnDestroy()
        {
            _applicationButton.OnClickButton -= OnClickApplicationBtn;
        }

        private void OnClickApplicationBtn()
        {
            SwitchPanel(DesctopPanelsType.ApplicationPanel);
        }
    }
}
