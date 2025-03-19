using System;
using BioEngineerLab.Activities;
using Core;
using UnityEngine;
using Database;

public class KeyChecker : MonoBehaviour
{
    public event Action KeyboardUnlockedEvent;
    
    private void OnTriggerEnter(Collider other)
    {
        GameManager gameManager = GameManager.Instance;
            
        if (gameManager == null)
        {
            return;
        }

        if (gameManager.CurrentBaseLocalManager == null)
        {
            return;
        }

        if (!other.CompareTag("Key"))
        {
            return;
        }
           
        AudioClip a = ResourcesDatabase.ReadSound("KeyChecker");
        AudioSource.PlayClipAtPoint(a, transform.position);
        gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnStart, EMachine.LaminBoxMachine));
        KeyboardUnlockedEvent?.Invoke();
    }
}
