using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class SocketActivity : Activity
    {
        public SocketType SocketType { get; private set; }
        public SocketActivityType SocketActivityType { get; private set; }

        public SocketActivity(SocketType socketType = SocketType.NeedleSocket, SocketActivityType socketActivityType = SocketActivityType.Enter)
            : base (EActivity.SocketActivity)
        {
            SocketType = socketType;
            SocketActivityType = socketActivityType;
        }

        public override void ShowInEditor()
        {
            SocketType = (SocketType) EditorGUILayout.EnumPopup("Socket Type", SocketType);
            SocketActivityType = (SocketActivityType) EditorGUILayout.EnumPopup("Socket Activity Type", SocketActivityType);
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not SocketActivity socketActivity)
            {
                return false;
            }
            
            return SocketType == socketActivity.SocketType &
                   SocketActivityType == socketActivity.SocketActivityType;
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