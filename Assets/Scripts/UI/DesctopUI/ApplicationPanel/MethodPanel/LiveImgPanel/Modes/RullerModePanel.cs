using System;
using BioEngineerLab.UI.Components;
using UnityEngine;

namespace BioEngineerLab.UI
{
    public class RullerModePanel : BasePanel<LiveImgModesPanelsType>
    {
        [SerializeField] private ButtonComponent _calibrationButton;

        private void Awake()
        {
            _calibrationButton.OnClickButton += OnClickCalibrationBtn;
        }

        private void OnDestroy()
        {
            _calibrationButton.OnClickButton -= OnClickCalibrationBtn;
        }

        private void OnClickCalibrationBtn()
        {
            SwitchPanel(LiveImgModesPanelsType.MainModePanel);
        }
    }
}