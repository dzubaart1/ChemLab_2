using System;
using Core;

namespace BioEngineerLab.Activities
{
    public class SocketLabActivity : LabActivity
    {
        public ESocket SocketType;
        public ESocketActivity SocketActivityType;

        public SocketLabActivity()
            : base(EActivity.SocketActivity)
        {
        }

        public SocketLabActivity(SocketLabActivity socketLabActivity)
            : base(EActivity.SocketActivity)
        {
            SocketType = socketLabActivity.SocketType;
            SocketActivityType = socketLabActivity.SocketActivityType;
        }
        
        public SocketLabActivity(ESocket socketType, ESocketActivity socketActivityType)
            : base(EActivity.SocketActivity)
        {
            SocketType = socketType;
            SocketActivityType = socketActivityType;
        }

        public override bool Equals(Object obj)
        {
            if (obj is not SocketLabActivity socketLabActivity)
            {
                return false;
            }

            return SocketType == socketLabActivity.SocketType &
                   SocketActivityType == socketLabActivity.SocketActivityType;
        }

        public override int GetHashCode()
        {
            return (int)SocketType + (int)SocketActivityType;
        }
    }
}