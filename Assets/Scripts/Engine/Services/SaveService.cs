using System;
using System.Threading.Tasks;
using BioEngineerLab.Core;
using UnityEngine;

public class SaveService : IService
{
    public event Action SaveSceneStateEvent;
    public event Action LoadSceneStateEvent;

    public Task Initialize()
    {
        return Task.CompletedTask;
    }

    public void Destroy()
    {
    }

    public void SaveSceneState()
    {
        Debug.Log("------SAVE------");
        SaveSceneStateEvent?.Invoke();
    }
    
    public void LoadSceneState()
    {
        Debug.Log("------LOAD------");
        LoadSceneStateEvent?.Invoke();
    }
}
