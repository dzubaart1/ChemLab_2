using System;

namespace BioEngineerLab.Activities
{
    public class SliderValueChangedActivity : Activity
    {
        public SliderType SliderType;
        public float Value;

        public SliderValueChangedActivity()
        {
            ActivityType = ActivityType.SliderValueChangedActivity;
        }
        
        public SliderValueChangedActivity(SliderType sliderType, float value)
        {
            ActivityType = ActivityType.SliderValueChangedActivity;
            SliderType = sliderType;
            Value = value;
        }
        
        public override bool EqualsActivity(Activity activity)
        {
            if (activity is not SliderValueChangedActivity sliderValueChanged)
            {
                return false;
            }
            
            return SliderType == sliderValueChanged.SliderType &
                   Math.Abs(Value - sliderValueChanged.Value) < 0.001f;
        }
    }

    public enum SliderType : byte
    {
        SyringeHeightSlider,
        BrightnessSlider,
        ContrastSlider,
        ZoomSlider,
        FocusSlider,
        FPSSlider,
        PlatformHeightSlider
    }
}