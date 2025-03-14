using Core;
using Saveables;
using UnityEngine;
using UI.Components;
using System;
using System.Collections.Generic;

namespace BioEngineerLab.Machines
{
    public class BoxPanelMachine : MonoBehaviour, ISaveableUI
    {
        private struct SavedData
        {
            public bool IsBactLightOn;
            public bool IsCommonLightOn;
            public bool IsDLightOn;
            public bool IsDoorOpened;
        }
        
        [Serializable]
        private struct MyLight
        {
            public Texture2D LightTexture;
            public Texture2D DirTexture;
        }
        
        [SerializeField] private ButtonComponent _bacteriumButton;
        [SerializeField] private ButtonComponent _lightButton;
        [SerializeField] private ButtonComponent _dlightButton;
        [SerializeField] private ButtonComponent _keyButton;

        /*[SerializeField] private Transform AddLight;*/
        
        [SerializeField] private MyLight _UVlight;
        [SerializeField] private MyLight _nonelight;
        [SerializeField] private MyLight _fulllight;
        [SerializeField] private MyLight _dlight;

        private LightmapData[] _noneLight;
        private LightmapData[] _UVLight;
        private LightmapData[] _DLight;
        private LightmapData[] _fullLight;
        
        private SavedData _savedData = new SavedData();
        
        private bool _isBactLightOn = true;
        private bool _isCommonLightOn = false;
        private bool _isDLightOn = true;
        private bool _isDoorOpened = false;
        
        private void Start()
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
            
            gameManager.CurrentBaseLocalManager.AddSaveableUI(this);
            
            List<LightmapData> uvlightmap = new List<LightmapData>();
            LightmapData uvlmdata = new LightmapData();
            uvlmdata.lightmapDir = _UVlight.DirTexture;
            uvlmdata.lightmapColor = _UVlight.LightTexture;
            uvlightmap.Add(uvlmdata);
            _UVLight = uvlightmap.ToArray();
            
            List<LightmapData> nonelightmap = new List<LightmapData>();
            LightmapData nonelmdata = new LightmapData();
            nonelmdata.lightmapDir = _nonelight.DirTexture;
            nonelmdata.lightmapColor = _nonelight.LightTexture;
            nonelightmap.Add(nonelmdata);
            _noneLight = nonelightmap.ToArray();
            
            List<LightmapData> fulllightmap = new List<LightmapData>();
            LightmapData fulllmdata = new LightmapData();
            fulllmdata.lightmapDir = _fulllight.DirTexture;
            fulllmdata.lightmapColor = _fulllight.LightTexture;
            fulllightmap.Add(fulllmdata);
            _fullLight = fulllightmap.ToArray();
            
            List<LightmapData> dlightmap = new List<LightmapData>();
            LightmapData dlmdata = new LightmapData();
            dlmdata.lightmapDir = _dlight.DirTexture;
            dlmdata.lightmapColor = _dlight.LightTexture;
            dlightmap.Add(dlmdata);
            _DLight = dlightmap.ToArray();
            
            SwitchDLight();
        }
        
        private void OnEnable()
        {
            _bacteriumButton.ClickBtnEvent += OnBacteriumButtonClicked;
            _lightButton.ClickBtnEvent += OnLightButtonClicked;
            _dlightButton.ClickBtnEvent += OnDLightButtonClicked;
            _keyButton.ClickBtnEvent += OnKeyButtonClicked;
        }

        private void OnDisable()
        {
            _bacteriumButton.ClickBtnEvent -= OnBacteriumButtonClicked;
            _lightButton.ClickBtnEvent -= OnLightButtonClicked;
            _dlightButton.ClickBtnEvent -= OnDLightButtonClicked;
            _keyButton.ClickBtnEvent -= OnKeyButtonClicked;
        }

        private void OnBacteriumButtonClicked()
        {
            _isBactLightOn = !_isBactLightOn;
            SwitchLightUV(_isBactLightOn);
        }

        private void OnLightButtonClicked()
        {
            _isCommonLightOn = true;
            _isDLightOn = false;
            
            SwitchFullLight();
        }
        
        private void OnDLightButtonClicked()
        {
            _isCommonLightOn = false;
            _isDLightOn = true;
            
            SwitchDLight();
        }

        private void OnKeyButtonClicked()
        {
            _isDoorOpened = !_isDoorOpened;
        }

        public void SaveUIState()
        {
            _savedData.IsBactLightOn = _isBactLightOn;
            _savedData.IsCommonLightOn = _isCommonLightOn;
            _savedData.IsDLightOn = _isDLightOn;
            _savedData.IsDoorOpened = _isDoorOpened;
        }

        public void LoadUIState()
        {
            _isBactLightOn = _savedData.IsBactLightOn;
            
            _isCommonLightOn = _savedData.IsCommonLightOn;
            
            _isDLightOn = _savedData.IsDLightOn;

            if (_isCommonLightOn)
            {
                SwitchFullLight();
            }
            else if (_isDLightOn)
            {
                SwitchDLight();
            }
            else
            {
                SwitchLightUV(_isBactLightOn);
            }
        }

        private void SwitchLightUV(bool isOn)
        {
            LightmapSettings.lightmaps = isOn ? _noneLight : _UVLight;
        }
        private void SwitchDLight()
        {
            LightmapSettings.lightmaps = _DLight;
        }

        private void SwitchFullLight()
        {
            LightmapSettings.lightmaps = _fullLight;
        }
    }
}
