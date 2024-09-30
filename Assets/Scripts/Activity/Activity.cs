using System;

namespace BioEngineerLab.Activities
{
    [Serializable]
    public abstract class Activity
    {
        public ActivityType ActivityType;

        public virtual bool EqualsActivity(Activity activity)
        {
            if (ActivityType != activity.ActivityType)
            {
                return false;
            }
            
            switch (ActivityType)
            {
                case ActivityType.MachineActivity:
                    (this as MachineActivity)?.Equals(activity);
                    break;
                case ActivityType.TransferActivity:
                    (this as TransferActivity)?.Equals(activity);
                    break;
                case ActivityType.ButtonClickedActivity:
                    (this as ButtonClickedActivity)?.Equals(activity);
                    break;
                case ActivityType.SliderValueChangedActivity:
                    (this as SliderValueChangedActivity)?.Equals(activity);
                    break;
                case ActivityType.AnchorActivity:
                    (this as AnchorActivity)?.Equals(activity);
                    break;
                case ActivityType.SocketActivity:
                    (this as SocketActivity)?.Equals(activity);
                    break;
                case ActivityType.InputFieldActivity:
                    (this as InputFieldActivity)?.Equals(activity);
                    break;
                case ActivityType.DropdownActivity:
                    (this as DropdownActivity)?.Equals(activity);
                    break;
                case ActivityType.DragLineActivity:
                    (this as DragLineActivity)?.Equals(activity);
                    break;
                case ActivityType.WashingActivity:
                    (this as WashingActivity)?.Equals(activity);
                    break;
            }

            return false;
        }
    }
    
    public enum ActivityType : byte
    {
        TransferActivity,
        MachineActivity,
        ButtonClickedActivity,
        SliderValueChangedActivity,
        SocketActivity,
        InputFieldActivity,
        DragLineActivity,
        DropdownActivity,
        AnchorActivity,
        WashingActivity
    }
}
