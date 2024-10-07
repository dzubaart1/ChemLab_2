using BioEngineerLab.Configurations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace BioEngineerLab.Core
{
    public class InputService : IService
    {
        public InputConfiguration Configuration { get; private set; }
        public Camera Camera { get; private set; }
        public LocalRig LocalRig { get; private set; } = null;

        public InputService(InputConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void Initialize()
        {
            Engine.CreateObject("InputManager", null, typeof(EventSystem), typeof(XRUIInputModule));
            
            LocalRig = Engine.Instantiate(Configuration.LocalRigTemplate);
            LocalRig.transform.position = Configuration.PlayerSpawnPoint;
            
            Camera = LocalRig.Camera;
        }

        public void Destroy()
        {

        }
    }
}
