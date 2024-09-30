using BioEngineerLab.Substances;

namespace BioEngineerLab.Activities
{
    public class AnchorActivity : Activity
    {
        public SubstanceName SubstanceName;
        
        public AnchorActivity()
        {
            ActivityType = ActivityType.AnchorActivity;
        }

        public AnchorActivity(SubstanceName substanceName)
        {
            SubstanceName = substanceName;
        }

        public override bool EqualsActivity(Activity activity)
        {
            if (activity is not AnchorActivity anchorActivity)
            {
                return false;
            }
            
            return (SubstanceName == anchorActivity.SubstanceName);
        }
    }
}