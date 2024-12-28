using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class KeyboardKey : MonoBehaviour
    {
        public event Action<int> ClickKeyboardKeyEvent;
        
        [Header("Configs")]
        [SerializeField] private int _value;

        [Space]
        [Header("Refs")]
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            ClickKeyboardKeyEvent?.Invoke(_value);
        }
    }
}