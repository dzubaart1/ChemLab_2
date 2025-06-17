using Core;
using Saveables;
using TMPro;
using UnityEngine;
using BioEngineerLab.Tasks.SideEffects;
using UnityEngine.UI;
using Database;

namespace Machines
{ 
    public class WarningTextActivator : MonoBehaviour, ISaveableOther, ISideEffectActivator
    {
        private class SavedData
        {
            public bool IsActive;
            public string Text;
            public bool IsButtonActive;
        }

        [Header("UIs")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;
        
        private bool _isActive = false;
        private bool _isButtonActive = false;
        private SavedData _savedData = new SavedData();

        public void Init()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.AddSaveableOther(this);
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
            
            _text.transform.gameObject.SetActive(false);
            _button.gameObject.SetActive(false);
        }
        
        public void Save()
        {
            _savedData.IsActive = _text.transform.gameObject.activeSelf;
            _savedData.Text = _text.text;
            _savedData.IsButtonActive = _button.gameObject.activeSelf;
        }

        public void Load()
        {
            _text.transform.gameObject.SetActive(_savedData.IsActive);
            _text.text = _savedData.Text;
        }

        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is WarningTextLabSideEffect warningTextLabSideEffect)
            {
                _text.transform.gameObject.SetActive(warningTextLabSideEffect.IsActive);
                _text.text = warningTextLabSideEffect.WarningText;
                if (warningTextLabSideEffect.IsActive)
                {
                    GameManager gameManager = GameManager.Instance;
                    if (gameManager == null)
                    {
                        return;
                    }
            
                    if (gameManager.IsSoundsOn)
                    {
                        AudioClip a = ResourcesDatabase.ReadSound("GetResult");
                        AudioSource.PlayClipAtPoint(a, transform.position, 0.7f);
                    }
                }
            }
            else if (sideEffect is TriggerActivatorSideEffect triggerActivatorSideEffect)
            {
                if (triggerActivatorSideEffect.TriggerType == ETriggerType.ContinueButtonTrigger)
                {
                    _button.gameObject.SetActive(triggerActivatorSideEffect.IsActive);
                    if (triggerActivatorSideEffect.IsActive)
                    {
                        GameManager gameManager = GameManager.Instance;
                        if (gameManager == null)
                        {
                            return;
                        }
            
                        if (gameManager.IsSoundsOn)
                        {
                            AudioClip a = ResourcesDatabase.ReadSound("GetResult");
                            AudioSource.PlayClipAtPoint(a, transform.position, 0.7f);
                        }
                    }
                }
            }
            
        }
    }
}