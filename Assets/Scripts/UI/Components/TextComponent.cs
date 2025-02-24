using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    [Serializable]
    public class TextConfig
    {
        public ELabLanguage Language;
        public string Text;
    }
    
    public class TextComponent : MonoBehaviour
    {
        [Header("UIs")]
        [SerializeField] private TMP_Text _text;

        [Space]
        [SerializeField] private List<TextConfig> _textConfigs;
        
        private void OnEnable()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            TextConfig targetConfig = _textConfigs.FirstOrDefault(config => config.Language == gameManager.CurrentLanguage);

            if (targetConfig == null)
            {
                Debug.LogError("Can't find config!");
                return;
            }

            _text.text = targetConfig.Text;
        }
    }
}