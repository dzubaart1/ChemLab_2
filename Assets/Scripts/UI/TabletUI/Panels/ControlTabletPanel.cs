using UnityEngine;
using UnityEngine.UI;

namespace BioEngineerLab.UI
{
    public class ControlTabletPanel : MonoBehaviour
    {
        [SerializeField] private Button _musicBtn, _soundsBtn, _hintBtn, _infoBtn, _warningBtn;
        [SerializeField] private TabletPanelSwitcher _tabletPanelSwitcher;
        
        private void Awake()
        {
            _musicBtn.onClick.AddListener(OnClickMusicBtn);
            _soundsBtn.onClick.AddListener(OnClickSoundsBtn);
            _hintBtn.onClick.AddListener(OnClickHintBtn);
            _infoBtn.onClick.AddListener(OnClickInfoBtn);
            _warningBtn.onClick.AddListener(OnClickWarningBtn);
        }

        private void OnDestroy()
        {
            _musicBtn.onClick.RemoveListener(OnClickMusicBtn);
            _soundsBtn.onClick.RemoveListener(OnClickSoundsBtn);
            _hintBtn.onClick.RemoveListener(OnClickHintBtn);
            _infoBtn.onClick.RemoveListener(OnClickInfoBtn);
            _warningBtn.onClick.RemoveListener(OnClickWarningBtn);
        }

        private void OnClickMusicBtn()
        {
            
        }

        private void OnClickSoundsBtn()
        {
            
        }

        private void OnClickInfoBtn()
        {
            Debug.Log("info");
            _tabletPanelSwitcher.SwitchPanel(TabletPanelsType.InfoTabletPanel);
        }

        private void OnClickHintBtn()
        {
            Debug.Log("hint");
            _tabletPanelSwitcher.SwitchPanel(TabletPanelsType.HintTabletPanel);
        }

        private void OnClickWarningBtn()
        {
            
        }
    }
}