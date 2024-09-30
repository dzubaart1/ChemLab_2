using System;

namespace BioEngineerLab.Activities
{
    public class DragLineActivity : Activity
    {
        public DragLineType DragLineType;
        public double YOffset;
        private const float ACCURACY = 2;

        public DragLineActivity()
        {
            ActivityType = ActivityType.DragLineActivity;
        }
        
        public DragLineActivity(DragLineType dragLineType, double yOffset)
        {
            ActivityType = ActivityType.DragLineActivity;
            DragLineType = dragLineType;
            YOffset = yOffset;
        }

        public override bool EqualsActivity(Activity activity)
        {
            if (activity is not DragLineActivity dragLineActivity)
            {
                return false;
            }
            
            return DragLineType == dragLineActivity.DragLineType &
                   Math.Abs(YOffset - dragLineActivity.YOffset) < ACCURACY;
        }
    }

    public enum DragLineType : byte
    {
        Ruler,
        SelectLine
    }
}