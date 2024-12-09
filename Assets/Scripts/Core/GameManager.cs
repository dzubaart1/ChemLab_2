using System;
using System.Collections;
using BioEngineerLab.Activities;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public event Action<string> LoadSceneCompleteEvent;
        
        [SerializeField] private Game _game;

        public Game Game => _game;
        
        [CanBeNull] public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        
        public void StartGame(ELab lab)
        {
            _game.StartGame(lab);
        }
        
        public void FinishGame()
        {
            _game.FinishGame();
        }

        public void SaveGame()
        {
            _game.SaveGame();
        }

        public void LoadGame()
        {
            _game.LoadGame();
        }

        public void CompleteTask(LabActivity activity)
        {
            _game.CompleteTask(activity);
        }
        
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation loadingScene = SceneManager.LoadSceneAsync(sceneName);
            
            while (!loadingScene.isDone)
            {
                yield return null;
            }
            
            LoadSceneCompleteEvent?.Invoke(sceneName);
        }
    }
}