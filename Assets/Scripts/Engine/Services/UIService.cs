using System;
using System.Threading.Tasks;
using BioEngineerLab.Configurations;
using BioEngineerLab.UI;
//using Microsoft.MixedReality.Toolkit.Experimental.UI;

namespace BioEngineerLab.Core
{
    public class UIService : IService
    {
        public UIConfiguration Configuration { get; private set; }
        //public NonNativeKeyboard Keyboard { get; private set; }
        
        public UIService(UIConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Initialize()
        {
            //return Task.CompletedTask;
        }

        public void Destroy()
        {
        }

        //public void RegisterKeyboard(NonNativeKeyboard keyboard)
        //{
        //    Keyboard = keyboard;
        //}
    }
}
