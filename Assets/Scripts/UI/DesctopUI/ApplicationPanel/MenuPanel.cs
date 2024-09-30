using BioEngineerLab.UI.Components;
using UnityEngine;

namespace BioEngineerLab.UI
{
    public class MenuPanel : BasePanel<ApplicationPanelsType>
    {
        [SerializeField] private ButtonComponent _methodBtn;
        [SerializeField] private ButtonComponent _createNewMeasurementBtn;

        private void Awake()
        {
            _methodBtn.OnClickButton += OnClickMethodBtn;
            _createNewMeasurementBtn.OnClickButton += OnClickCreateNewMeasurementBtn;
        }

        private void OnDestroy()
        {
            _methodBtn.OnClickButton -= OnClickMethodBtn;
            _createNewMeasurementBtn.OnClickButton -= OnClickCreateNewMeasurementBtn;
        }

        private void OnClickMethodBtn()
        {
        }

        private void OnClickCreateNewMeasurementBtn()
        {
            SwitchPanel(ApplicationPanelsType.MethodPanel);
        }
    }
}
