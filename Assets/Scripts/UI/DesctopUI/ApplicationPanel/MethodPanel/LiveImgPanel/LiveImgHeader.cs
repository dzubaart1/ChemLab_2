using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.UI.Components;
using UnityEngine;

public class LiveImgHeader : MonoBehaviour, ISaveable
{
    private struct SavedData
    {
        public bool IsSettingsPanelVisible;
    }
    
    [SerializeField] private ButtonComponent _settingsBtn;
    [SerializeField] private SettingsPanel _settingsPanel;

    private SaveService _saveService;

    private SavedData _savedData;
    
    private void Awake()
    {
        _saveService = Engine.GetService<SaveService>();
        _saveService.LoadSceneStateEvent += OnLoadScene;
        _saveService.SaveSceneStateEvent += OnSaveScene;

        _settingsBtn.OnClickButton += OnClickSettingsBtn;

        _savedData = new SavedData();
    }

    private void OnDestroy()
    {
        _saveService.LoadSceneStateEvent -= OnLoadScene;
        _saveService.SaveSceneStateEvent -= OnSaveScene;
        
        _settingsBtn.OnClickButton -= OnClickSettingsBtn;
    }

    public void OnSaveScene()
    {
        _savedData.IsSettingsPanelVisible = _settingsPanel.gameObject.activeSelf;
    }

    public void OnLoadScene()
    {
        _settingsPanel.gameObject.SetActive(_savedData.IsSettingsPanelVisible);
    }

    private void OnClickSettingsBtn()
    {
        _settingsPanel.gameObject.SetActive(!_settingsPanel.gameObject.activeSelf);
    }
}
