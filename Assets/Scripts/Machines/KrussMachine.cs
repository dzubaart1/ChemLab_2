using BioEngineerLab.Core;
using UnityEngine;

namespace BioEngineerLab.Machines
{
    public class KrussMachine : MonoBehaviour
    {
        [SerializeField] private Light _light;
        [SerializeField] private GameObject _syringeHolder;
        [SerializeField] private GameObject _platform;
        [SerializeField] private float _speedMoving;

        private MachinesService _machinesService;
        private SaveService _saveService;
        
        private float _syringeSliderValue;
        private float _platformHeightSliderValue;

        private void Awake()
        {
            _machinesService = Engine.GetService<MachinesService>();
            _machinesService.RegisterKrussMachine(this);
            SetSyringeHeight(40);
            SetPlatformHeight(10);
        }
        private void Update()
        {
            float step = _speedMoving * Time.deltaTime;

            float syringeYValue = ((_syringeSliderValue -25)/150) +0.7f;
            Vector3 syringeTargetPos = _syringeHolder.transform.localPosition;
            syringeTargetPos.y = syringeYValue;
            
            _syringeHolder.transform.localPosition = Vector3.MoveTowards(_syringeHolder.transform.localPosition, syringeTargetPos, step);

            
            float platformYValue = ((_platformHeightSliderValue - 10) / 250) + 0.46f;
            Vector3 platformTargetPos = _platform.transform.localPosition;
            platformTargetPos.y = platformYValue;
            
            _platform.transform.localPosition = Vector3.MoveTowards(_platform.transform.localPosition, platformTargetPos, step);
        }

        public void SetBrightness(float brightness)
        {
            _light.intensity = brightness;
        }

        public void SetSyringeHeight(float _syringeSlider)
        {
            _syringeSliderValue = _syringeSlider; 
        }

        public void SetPlatformHeight(float _platformHeightSlider)
        {
            _platformHeightSliderValue = _platformHeightSlider;
        }
    }
}