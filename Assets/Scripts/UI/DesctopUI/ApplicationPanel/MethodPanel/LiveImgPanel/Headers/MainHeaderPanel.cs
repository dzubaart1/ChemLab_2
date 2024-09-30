using BioEngineerLab.Core;
using BioEngineerLab.UI.Components;
using UnityEngine;

namespace BioEngineerLab.UI.Headers
{
    public class MainHeaderPanel :BasePanel<LiveImgHeaderPanelsType>
    {
        [SerializeField] private ButtonComponent _rulerModeButton;
        [SerializeField] private ButtonComponent _pauseButton;
        [SerializeField] private ModesPanelSwitcher _modesPanelSwitcher;
        [SerializeField] private DragLine _dragLine;

        private DropAnimationService _dropAnimationService;
        private void Awake()
        {
            _dropAnimationService = Engine.GetService<DropAnimationService>();
            
            _rulerModeButton.OnClickButton += OnClickRulerModeBtn;
            _pauseButton.OnClickButton += OnClickPauseBtn;
        }

        private void OnDestroy()
        {
            _rulerModeButton.OnClickButton -= OnClickRulerModeBtn;
            _pauseButton.OnClickButton -= OnClickPauseBtn;

        }
        
        private void OnClickRulerModeBtn()
        {
            _modesPanelSwitcher.SwitchPanel(LiveImgModesPanelsType.RulerModePanel);
        }
        
        private void OnClickPauseBtn()
        {
            SwitchPanel(LiveImgHeaderPanelsType.ChooseFrameHeaderPanel);
            _dropAnimationService.ShowFirstFrame();
            _dragLine.gameObject.SetActive(false);
            Debug.Log("dragLine hide pause");
        }
    }
}