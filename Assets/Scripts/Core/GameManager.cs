using System;
using System.Collections;
using JetBrains.Annotations;
using LocalManagers;
using UI.TabletUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public const string LOBBY_SCENE_NAME = "LobbyScene";
        public const string SPACE_LAB_SCENE_NAME = "SpaceLab";
        public const string CUBE_LAB_SCENE_NAME = "CubeLab";
        
        public event Action<string> LoadSceneCompleteEvent;
        
        [CanBeNull] public static GameManager Instance { get; private set; }
        [CanBeNull] public BaseLocalManager CurrentBaseLocalManager { get; private set; }
        
        public ELab CurrentLab { get; private set; }
        public float GameTime { get; private set; }
        public int ErrorsCount { get; private set; }
        public bool IsGameFinished { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void OnFinishGame(float gameTime, int errorsCount)
        {
            GameTime = gameTime;
            ErrorsCount = errorsCount;
            IsGameFinished = true;
        }

        public void SetLocalManger(BaseLocalManager localManager)
        {
            CurrentBaseLocalManager = localManager;
        }

        public void OnSelectLab(ELab lab)
        {
            TabletUI tabletUI = FindObjectOfType<TabletUI>();
            if (tabletUI == null)
            {
                return;
            }
            
            tabletUI.OnSelectLab(lab);
        }

        public void SetLab(ELab lab)
        {
            CurrentLab = lab;
            
            switch (lab)
            {
                case ELab.Lab1:
                    LoadScene(SPACE_LAB_SCENE_NAME);
                    break;
                case ELab.Lab2:
                    LoadScene(CUBE_LAB_SCENE_NAME);
                    break;
                case ELab.Lab3:
                    LoadScene(CUBE_LAB_SCENE_NAME);
                    break;
            }
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