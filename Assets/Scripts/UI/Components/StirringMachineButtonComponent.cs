using System;
using BioEngineerLab.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class StirringMachineButtonComponent : ButtonComponent
    {
        [HideInInspector]
        public bool IsOn;

        [SerializeField] private Image _btnImage;
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;

        protected override void OnClickBtn()
        {
            IsOn = !IsOn;
            UpdateSprite();
            
            base.OnClickBtn();
        }
        
        private void UpdateSprite()
        {
            _btnImage.sprite = IsOn ? _onSprite : _offSprite;
        }

        public void OnLoadScene(bool state)
        {
            IsOn = state;
            UpdateSprite();
        }
    }
}