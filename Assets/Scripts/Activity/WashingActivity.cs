using BioEngineerLab.Containers;
namespace BioEngineerLab.Activities
{
    public class WashingActivity : Activity
    {
        public ContainerType Container;

        public WashingActivity()
        {
            ActivityType = ActivityType.WashingActivity;
        }
        
        public WashingActivity(ContainerType container)
        {
            ActivityType = ActivityType.WashingActivity;
            Container = container;
        }

        public override bool EqualsActivity(Activity activity)
        {
            if (activity is not WashingActivity washingActivity)
            {
                return false;
            }

            return washingActivity.Container == Container;
        }
    }

}