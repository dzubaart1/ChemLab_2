using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class SocketActivity : Activity
    {
        private SocketType _socketType;
        private SocketActivityType _socketActivityType;

        public SocketActivity(SocketType socketType = SocketType.NeedleSocket, SocketActivityType socketActivityType = SocketActivityType.Enter)
            : base (EActivity.SocketActivity)
        {
            _socketType = socketType;
            _socketActivityType = socketActivityType;
        }

        public override void ShowInEditor()
        {
            _socketType = (SocketType) EditorGUILayout.EnumPopup("Socket Type", _socketType);
            _socketActivityType = (SocketActivityType) EditorGUILayout.EnumPopup("Socket Activity Type", _socketActivityType);
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not SocketActivity socketActivity)
            {
                return false;
            }
            
            return _socketType == socketActivity._socketType &
                   _socketActivityType == socketActivity._socketActivityType;
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