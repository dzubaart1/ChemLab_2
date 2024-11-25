using System;

namespace Core.Services
{
    public class SaveService : IService
    {
        public event Action SaveSceneStateEvent;
        public event Action LoadSceneStateEvent;

        public void Initialize()
        {
        }

        public void Destroy()
        {
        }

        public void SaveSceneState()
        {
            SaveSceneStateEvent?.Invoke();
        }
    
        public void LoadSceneState()
        {
            LoadSceneStateEvent?.Invoke();
        }
    }
}
