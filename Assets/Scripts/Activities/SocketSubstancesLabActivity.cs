using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Substances;

namespace Activities
{
    public class SocketSubstancesLabActivity : LabActivity
    {
        public ESocket SocketType;
        public ESocketActivity SocketActivityType;
        public LabSubstanceProperty[] LabSubstanceProperties = Array.Empty<LabSubstanceProperty>();

        public SocketSubstancesLabActivity()
            : base(EActivity.SocketSubstancesActivity)
        {
        }

        public SocketSubstancesLabActivity(SocketSubstancesLabActivity socketSubstancesLabActivity)
            : base(EActivity.SocketSubstancesActivity)
        {
            SocketType = socketSubstancesLabActivity.SocketType;
            SocketActivityType = socketSubstancesLabActivity.SocketActivityType;
        }

        public SocketSubstancesLabActivity(ESocket socketType, ESocketActivity socketActivityType, IReadOnlyCollection<LabSubstanceProperty> labSubstanceProperties)
            : base (EActivity.SocketSubstancesActivity)
        {
            SocketType = socketType;
            SocketActivityType = socketActivityType;
            LabSubstanceProperties = labSubstanceProperties.ToArray();
        }

        public override bool Equals(object obj)
        {
            if (obj is not SocketSubstancesLabActivity handlerSocketActivity)
            {
                return false;
            }

            return SocketType == handlerSocketActivity.SocketType &
                   SocketActivityType == handlerSocketActivity.SocketActivityType &
                   LabSubstanceProperties.All(handlerSocketActivity.LabSubstanceProperties.Contains) &
                   handlerSocketActivity.LabSubstanceProperties.All(LabSubstanceProperties.Contains);
        }

        public override int GetHashCode()
        {
            int sum = (int)SocketType + (int)SocketActivityType;

            foreach (var substanceProperty in LabSubstanceProperties)
            {
                sum += substanceProperty.GetHashCode();
            }
            
            return sum;
        }
    }
}