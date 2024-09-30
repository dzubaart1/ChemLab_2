using System.Collections.Generic;
using BioEngineerLab.Core;
using BioEngineerLab.UI;
using BioEngineerLab.UI.Components;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private SliderComponent _brightnessSlider;
    [SerializeField] private SliderComponent _contrastSlider;
    [SerializeField] private SliderComponent _zoomSlider;
    [SerializeField] private SliderComponent _focusSlider;
    [SerializeField] private SliderComponent _fpsSlider;
  
    [Space]
    [Header("Others References")]
    [SerializeField] private List<Sprite> _zoomSprites;
    [SerializeField] private LiveImage _liveImage;
    
    private MachinesService _machinesService;

    private void Awake()
    {
        _machinesService = Engine.GetService<MachinesService>();
        
        _brightnessSlider.ValueChangedEvent += BrightnessChangedValue;
        _contrastSlider.ValueChangedEvent += ContrastChangedValue;
        _zoomSlider.ValueChangedEvent += ZoomChangedValue;
        _focusSlider.ValueChangedEvent += FocusChangedValue;
        _fpsSlider.ValueChangedEvent += FPSChangedValue;
    }

    private void OnDestroy()
    {
        _brightnessSlider.ValueChangedEvent -= BrightnessChangedValue;
        _contrastSlider.ValueChangedEvent -= ContrastChangedValue;
        _zoomSlider.ValueChangedEvent -= ZoomChangedValue;
        _focusSlider.ValueChangedEvent -= FocusChangedValue;
        _fpsSlider.ValueChangedEvent -= FPSChangedValue;
    }
    
    private void BrightnessChangedValue(float value)
    {
        _machinesService.KrussMachine.SetBrightness(value / 50);
    }

    private void ContrastChangedValue(float value)
    {
        _liveImage.ChangeContrast(value*1.8f/100);
    }

    private void ZoomChangedValue(float value)
    {
        var intValue = (int)value - 25;
        _liveImage.ChangeSprite( _zoomSprites[intValue]);
    }

    private void FocusChangedValue(float value)
    {

    }

    private void FPSChangedValue(float value)
    {

    }
}
