namespace BioEngineerLab.Activities
{
    public class SocketActivity : Activity
    {
        public SocketType SocketType;
        public SocketActivityType SocketActivityType;

        public SocketActivity()
        {
            ActivityType = ActivityType.SocketActivity;
        }
        
        public SocketActivity(SocketType socketType, SocketActivityType socketActivityType)
        {
            ActivityType = ActivityType.SocketActivity;
            SocketType = socketType;
            SocketActivityType = socketActivityType;
        }
        
        public override bool EqualsActivity(Activity activity)
        {
            if (activity is not SocketActivity socketActivity)
            {
                return false;
            }
            
            return SocketType == socketActivity.SocketType;
        }
    }
    
    public enum SocketActivityType : byte
    {
        Enter,
        Exit
    }
    
    public enum SocketType : byte
    {
        PistonSocket,
        NeedleSocket,
        KRUSSSyringeSocket,
        KRUSSKuvetkaSocket,
        PAFBodySocket,
        HeptanBodySocket,
        KRUSSInteractablePanelSocket,
        StirringMachineSocket,
        LabCoatSocket,
    }
}