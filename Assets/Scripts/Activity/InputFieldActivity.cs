using System;

namespace BioEngineerLab.Activities
{
    public class InputFieldActivity : Activity
    {
        public InputFieldType InputFieldType;
        public float Value;

        public InputFieldActivity()
        {
            ActivityType = ActivityType.InputFieldActivity;
        }
        
        public InputFieldActivity(InputFieldType inputFieldType, float value)
        {
            ActivityType = ActivityType.InputFieldActivity;
            InputFieldType = inputFieldType;
            Value = value;
        }

        public override bool EqualsActivity(Activity activity)
        {
            if (activity is not InputFieldActivity inputFieldActivity)
            {
                return false;
            }
            
            return InputFieldType == inputFieldActivity.InputFieldType &
                   Math.Abs(Value - inputFieldActivity.Value) < 0.01f;
        }
    }
    
    public enum InputFieldType : byte
    {
        RulerModeInputField,
        notebookPNInHeptanField,
        notebookPNInHeptanPAFField,
    }
}