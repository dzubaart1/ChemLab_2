using BioEngineerLab.Core;
using BioEngineerLab.Machines;
using BioEngineerLab.UI.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformAxesPanel : MonoBehaviour
{
    [SerializeField] private SliderComponent _platformHeightSlider;

    private MachinesService _machinesService;
    private KrussMachine _krussMachine;

    private void Awake()
    {
        _machinesService = Engine.GetService<MachinesService>();

        _platformHeightSlider.ValueChangedEvent += PlatformHeightChangedValue;
    }

    private void OnDestroy()
    {
        _platformHeightSlider.ValueChangedEvent -= PlatformHeightChangedValue;
    }

    private void Start()
    {
        _krussMachine = _machinesService.KrussMachine;
    }

    private void PlatformHeightChangedValue(float value)
    {
        _krussMachine.SetPlatformHeight(value);
    }
}
