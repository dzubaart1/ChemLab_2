using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.Activities;

namespace BioEngineerLab.Activities
{
    public class SocketLabActivity : LabActivity
    {
        public ESocket SocketType;
        public ESocketActivity SocketActivityType;
        public EContainer Container;

        public SocketLabActivity()
            : base(EActivity.SocketActivity)
        {
        }

        public SocketLabActivity(SocketLabActivity socketLabActivity)
            : base(EActivity.SocketActivity)
        {
            SocketType = socketLabActivity.SocketType;
            SocketActivityType = socketLabActivity.SocketActivityType;
            Container = socketLabActivity.Container;
        }

        public SocketLabActivity(ESocket socketType, ESocketActivity socketActivityType, EContainer container)
            : base (EActivity.SocketActivity)
        {
            SocketType = socketType;
            SocketActivityType = socketActivityType;
            Container = container;
        }

        public override bool Equals(object obj)
        {
            if (obj is not SocketLabActivity handlerSocketActivity)
            {
                return false;
            }

            return SocketType == handlerSocketActivity.SocketType &
                   SocketActivityType == handlerSocketActivity.SocketActivityType;
        }

        public override int GetHashCode()
        {
            return (int)SocketType + (int)SocketActivityType + (int)Container;
        }
    }
}