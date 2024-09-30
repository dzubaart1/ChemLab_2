using BioEngineerLab.Core;
using BioEngineerLab.Machines;
using BioEngineerLab.UI.Components;
using UnityEngine;

public class SyringeDozingPanel : MonoBehaviour
{
    [SerializeField] private SliderComponent _syringeHeightSlider;

    private MachinesService _machinesService;
    private KrussMachine _krussMachine;

    private void Awake()
    {
        _machinesService = Engine.GetService<MachinesService>();

        _syringeHeightSlider.ValueChangedEvent += SyringeHeightChangedValue;
    }
    
    private void OnDestroy()
    {
        _syringeHeightSlider.ValueChangedEvent -= SyringeHeightChangedValue;
    }

    private void Start()
    {
        _krussMachine = _machinesService.KrussMachine;
    }

    private void SyringeHeightChangedValue(float value)
    {
        _krussMachine.SetSyringeHeight(value);
    }
}
