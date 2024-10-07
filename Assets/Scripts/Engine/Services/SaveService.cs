using System;
using System.Threading.Tasks;
using BioEngineerLab.Core;
using UnityEngine;

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
