using System;
using Core;

namespace BioEngineerLab.Activities
{
    public class ButtonClickedActivity : LabActivity
    {
        public EButton ButtonType;

        public ButtonClickedActivity()
            : base(EActivity.ButtonClickedActivity)
        {
        }

        public ButtonClickedActivity(ButtonClickedActivity buttonClickedActivity)
            : base(EActivity.ButtonClickedActivity)
        {
            ButtonType = buttonClickedActivity.ButtonType;
        }
        
        public ButtonClickedActivity(EButton buttonType)
            : base(EActivity.MachineActivity)
        {
            ButtonType = buttonType;
        }

        public override bool Equals(Object obj)
        {
            if (obj is not ButtonClickedActivity buttonClickedActivity)
            {
                return false;
            }

            return ButtonType == buttonClickedActivity.ButtonType;
        }

        public override int GetHashCode()
        {
            return (int)ButtonType;
        }
    }
}