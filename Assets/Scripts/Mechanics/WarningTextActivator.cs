using Core;
using Saveables;
using TMPro;
using UnityEngine;
using BioEngineerLab.Tasks.SideEffects;

namespace Machines
{ 
    public class WarningTextActivator : MonoBehaviour, ISaveableOther, ISideEffectActivator
    {
        private class SavedData
        {
            public bool IsActive;
            public string Text;
        }

        [Header("UIs")]
        [SerializeField] private TextMeshProUGUI _text;
        
        private bool _isActive = false;
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
        }
        
        public void Save()
        {
            _savedData.IsActive = _text.transform.gameObject.activeSelf;
            _savedData.Text = _text.text;
        }

        public void Load()
        {
            _text.transform.gameObject.SetActive(_savedData.IsActive);
            _text.text = _savedData.Text;
        }

        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not WarningTextLabSideEffect warningTextLabSideEffect)
            {
                return;
            }

            _text.transform.gameObject.SetActive(warningTextLabSideEffect.IsActive);
            _text.text = warningTextLabSideEffect.WarningText;
        }
    }
}