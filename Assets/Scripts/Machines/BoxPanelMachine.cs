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
            public Texture2D ShadowTexture;
            public Texture2D DirTexture;
        }
        
        [SerializeField] private ButtonComponent _bacteriumButton;
        [SerializeField] private ButtonComponent _lightButton;
        [SerializeField] private ButtonComponent _dlightButton;
        [SerializeField] private ButtonComponent _keyButton;
        
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
            uvlmdata.shadowMask = _UVlight.ShadowTexture;
            _UVLight = uvlightmap.ToArray();
            
            List<LightmapData> nonelightmap = new List<LightmapData>();
            LightmapData nonelmdata = new LightmapData();
            nonelmdata.lightmapDir = _nonelight.DirTexture;
            nonelmdata.lightmapColor = _nonelight.LightTexture;
            nonelmdata.shadowMask = _nonelight.ShadowTexture;
            nonelightmap.Add(nonelmdata);
            _noneLight = nonelightmap.ToArray();
            
            List<LightmapData> fulllightmap = new List<LightmapData>();
            LightmapData fulllmdata = new LightmapData();
            fulllmdata.lightmapDir = _fulllight.DirTexture;
            fulllmdata.lightmapColor = _fulllight.LightTexture;
            fulllmdata.shadowMask = _fulllight.ShadowTexture;
            fulllightmap.Add(fulllmdata);
            _fullLight = fulllightmap.ToArray();
            
            List<LightmapData> dlightmap = new List<LightmapData>();
            LightmapData dlmdata = new LightmapData();
            dlmdata.lightmapDir = _dlight.DirTexture;
            dlmdata.lightmapColor = _dlight.LightTexture;
            dlmdata.shadowMask = _dlight.ShadowTexture;
            dlightmap.Add(dlmdata);
            _DLight = dlightmap.ToArray();
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
            LightmapSettings.lightmaps = _isBactLightOn ? _UVLight : _noneLight;
        }

        private void OnLightButtonClicked()
        {
            _isCommonLightOn = true;
            _isDLightOn = false;
            
            LightmapSettings.lightmaps = _fullLight;
        }
        
        private void OnDLightButtonClicked()
        {
            _isCommonLightOn = false;
            _isDLightOn = true;
            
            LightmapSettings.lightmaps = _DLight;
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
            
            LightmapSettings.lightmaps = _isCommonLightOn ? _fullLight : _DLight ;
        }
    }
}
