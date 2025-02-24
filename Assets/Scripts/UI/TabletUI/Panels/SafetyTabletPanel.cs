using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class SafetyTabletPanel : BaseTabletPanel
    {
        [SerializeField] private TextMeshProUGUI _taskSafetyText;
        [SerializeField] private Button _returnButton;
        
        [CanBeNull] private LabTask _showingTask;

        private void OnEnable()
        {
            _returnButton.onClick.AddListener(OnReturnButtonClicked);
        }

        private void OnDisable()
        {
            _returnButton.onClick.RemoveListener(OnReturnButtonClicked);
        }

        private void OnReturnButtonClicked()
        {
            TabletUI.SwitchToMainPanel();
        }

        public override void SetTaskToShow(LabTask task)
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            if (task == null)
            {
                return;
            }
            
            _showingTask = task;
            
            switch (gameManager.CurrentLanguage)
            {
                case ELabLanguage.English:
                    _taskSafetyText.text = _showingTask.SafetyPrecautionsEnglish;
                    break;
                case ELabLanguage.Russia:
                    _taskSafetyText.text = _showingTask.SafetyPrecautions;
                    break;
                default:
                    Debug.LogError("Can't find language!");
                    break;
            }
        }

        public override void SetLabToShow(ELab lab)
        {
        }
    }
}