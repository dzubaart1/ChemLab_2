using System.Threading.Tasks;
using BioEngineerLab.Configurations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.VFX;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace BioEngineerLab.Core
{
    public class InputService : IService
    {
        public ETargetPlatform Platform = ETargetPlatform.Auto;

        public InputConfiguration Configuration { get; private set; }
        public Camera Camera { get; private set; }
        public LocalRig LocalRig { get; private set; } = null;

        public InputService(InputConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public Task Initialize()
        {
            if (Configuration.Platform == ETargetPlatform.Auto)
                Configuration.Platform = Application.platform == RuntimePlatform.Android ? ETargetPlatform.VR : ETargetPlatform.PC;

            Platform = Configuration.Platform;

            Engine.CreateObject("InputManager", null, typeof(EventSystem), typeof(XRUIInputModule));
            
            LocalRig = Engine.Instantiate(Configuration.LocalRigTemplate);
            LocalRig.transform.position = Configuration.PlayerSpawnPoint;
            
            Camera = LocalRig.Camera;

            return Task.CompletedTask;
        }

        public void Destroy()
        {

        }
    }
}
