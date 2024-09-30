using System;
using BioEngineerLab.UI.Components;
using UnityEngine;

namespace BioEngineerLab.UI
{
    public class WarningPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _drugLine;
        [SerializeField] private ButtonComponent _okButton;

        private void Awake()
        {
            _okButton.OnClickButton += OnClickOkBtn;
        }

        private void OnDestroy()
        {
            _okButton.OnClickButton -= OnClickOkBtn;
        }
        
        private void OnClickOkBtn()
        {
            gameObject.SetActive(false);
            _drugLine.SetActive(true);  
        }
    }
}