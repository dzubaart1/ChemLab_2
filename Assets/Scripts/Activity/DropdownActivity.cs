namespace BioEngineerLab.Activities
{
    public class DropdownActivity : Activity
    {
        public DropdownType DropdownType;
        public int Value;

        public DropdownActivity()
        {
            ActivityType = ActivityType.DropdownActivity;
        }
        
        public DropdownActivity(DropdownType dropdownType, int value)
        {
            ActivityType = ActivityType.DropdownActivity;
            DropdownType = dropdownType;
            Value = value;
        }

        public override bool EqualsActivity(Activity activity)
        {
            if (activity is not DropdownActivity dropdownActivity)
            {
                return false;
            }

            return DropdownType == dropdownActivity.DropdownType &
                   dropdownActivity.Value == Value;
        }
    }

    public enum DropdownType
    {
        RulerModeDropdown,
        Dropdown2,
        Dropdown3,
    }
}