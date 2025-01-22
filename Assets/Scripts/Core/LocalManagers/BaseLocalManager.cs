using System.Collections;
using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using Crafting;
using Saveables;
using UnityEngine;

namespace LocalManagers
{
    public abstract class BaseLocalManager : MonoBehaviour
    {
        private void Awake()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            gameManager.SetLocalManger(this);
        }

        private IEnumerator Start()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                yield break;
            }
            
            StartCoroutine(InitLab(gameManager.CurrentLab));
        }
        
        public abstract IReadOnlyList<SOLabCraft> GetSOCrafts();

        public abstract void AddSaveableUI(ISaveableUI saveableUI);
        public abstract void AddSaveableOther(ISaveableOther saveableOther);
        public abstract void AddSaveableDoor(ISaveableDoor saveableDoor);
        public abstract void AddSideEffectActivator(ISideEffectActivator sideEffectActivator);
        public abstract void AddSaveableContainer(ISaveableContainer saveableContainer);
        public abstract void AddSaveableSocket(ISaveableSocket saveableSocket);
        public abstract void AddGrabInteractables(ISaveableGrabInteractable saveableGrabInteractable);

        public abstract IEnumerator InitLab(ELab lab);
        public abstract void FinishGame();
        public abstract void SaveGame();
        public abstract void LoadGame();
        public abstract void OnActivityComplete(LabActivity activity);
    }
}